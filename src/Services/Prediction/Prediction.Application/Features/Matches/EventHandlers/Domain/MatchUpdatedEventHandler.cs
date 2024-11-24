using MediatR;
using Prediction.Domain.Events;

namespace Prediction.Application.Features.Matches.EventHandlers.Domain
{
    public class MatchUpdatedEventHandler(IApplicationDbContext context, IMediator mediator, ILogger<MatchUpdatedEventHandler> logger) : INotificationHandler<MatchUpdatedEvent>
    {
        public async Task Handle(MatchUpdatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("MatchUpdatedEventHandler triggered for MatchId: {MatchId}", notification.MatchId);

            // Fetch the match
            var match = await context.Matches.FindAsync(new object[] { notification.MatchId }, cancellationToken);
            if (match == null)
            {
                logger.LogWarning("Match with ID {MatchId} not found.", notification.MatchId);
                throw new ObjectNotFoundException(notification.MatchId.Value, "Match not found.");
            }

            // Fetch existing TeamHistories for the specific match
            var existingHistories = await context.TeamHistories
                .Where(th =>
                    th.TeamId == match.Team1Id && th.OpponentId == match.Team2Id && th.Date == match.Date ||
                    th.TeamId == match.Team2Id && th.OpponentId == match.Team1Id && th.Date == match.Date)
                .ToListAsync(cancellationToken);

            // Remove outdated histories
            if (existingHistories.Any())
            {
                logger.LogInformation("Removing {Count} outdated histories for MatchId {MatchId}.", existingHistories.Count, notification.MatchId);
                context.TeamHistories.RemoveRange(existingHistories);
            }

            // Fetch dynamic OutcomeTypes
            var outcomeTypes = await context.OutcomeTypes.ToListAsync(cancellationToken);
            if (!outcomeTypes.Any())
            {
                logger.LogWarning("No OutcomeTypes found in the system.");
                throw new InvalidOperationException("No OutcomeTypes available for calculating TeamHistories.");
            }

            // Recalculate outcomes for both teams
            var outcomesForTeam1 = await match.GetApplicableOutcomesAsync(context, match.Team1Id, cancellationToken);
            var outcomesForTeam2 = await match.GetApplicableOutcomesAsync(context, match.Team2Id, cancellationToken);

            // Add updated TeamHistory entries for both teams
            var newHistories = new List<TeamHistory>();

            newHistories.AddRange(outcomesForTeam1.Select(outcome =>
                TeamHistory.Create(
                    TeamHistoryId.Of(Guid.NewGuid()),
                    match.Team1Id,
                    match.Team2Id,
                    match.Date,
                    match.Team1Goals ?? 0,
                    match.Team2Goals ?? 0,
                    outcome
                )));

            newHistories.AddRange(outcomesForTeam2.Select(outcome =>
                TeamHistory.Create(
                    TeamHistoryId.Of(Guid.NewGuid()),
                    match.Team2Id,
                    match.Team1Id,
                    match.Date,
                    match.Team2Goals ?? 0,
                    match.Team1Goals ?? 0,
                    outcome
                )));

            logger.LogInformation("Adding {Count} new histories for MatchId {MatchId}.", newHistories.Count, notification.MatchId);

            context.TeamHistories.AddRange(newHistories);

            // Save changes
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("MatchUpdatedEventHandler successfully processed for MatchId {MatchId}.", notification.MatchId);

            // Trigger ForecastEvaluationCreatedEvent if the match is completed
            if (match.IsCompleted)
            {
                var forecastEvaluationEvent = new ForecastEvaluationCreatedEvent(match.Id);
                await mediator.Publish(forecastEvaluationEvent, cancellationToken);
                logger.LogInformation("ForecastEvaluationCreatedEvent published for MatchId: {MatchId}.", match.Id.Value);
            }
        }
    }
}
