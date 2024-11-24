using MediatR;
using Prediction.Domain.Events;

namespace Prediction.Application.Features.Matches.EventHandlers.Domain
{
    public class MatchCreatedEventHandler(IApplicationDbContext context, ILogger<MatchCreatedEventHandler> logger) : INotificationHandler<MatchCreatedEvent>
    {
        public async Task Handle(MatchCreatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("MatchCreatedEventHandler triggered for MatchId: {MatchId}", notification.MatchId);

            // Fetch the match
            var match = await context.Matches.FindAsync(new object[] { notification.MatchId }, cancellationToken);
            if (match == null)
            {
                logger.LogWarning("Match with ID {MatchId} not found.", notification.MatchId);
                throw new ObjectNotFoundException(notification.MatchId.Value, "Match not found.");
            }

            // Ensure the match is completed before processing
            if (!match.IsCompleted)
            {
                logger.LogInformation("Match with ID {MatchId} is not completed. Skipping TeamHistories update.", notification.MatchId);
                return;
            }

            // Fetch dynamic OutcomeTypes
            var outcomeTypes = await context.OutcomeTypes.ToListAsync(cancellationToken);
            if (!outcomeTypes.Any())
            {
                logger.LogWarning("No OutcomeTypes found in the system.");
                throw new InvalidOperationException("No OutcomeTypes available for calculating TeamHistories.");
            }

            // Calculate outcomes for both teams
            var outcomesForTeam1 = await match.GetApplicableOutcomesAsync(context, match.Team1Id, cancellationToken);
            var outcomesForTeam2 = await match.GetApplicableOutcomesAsync(context, match.Team2Id, cancellationToken);

            // Create TeamHistory entries for both teams
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

            logger.LogInformation("MatchCreatedEventHandler successfully processed for MatchId {MatchId}.", notification.MatchId);
        }
    }
}
