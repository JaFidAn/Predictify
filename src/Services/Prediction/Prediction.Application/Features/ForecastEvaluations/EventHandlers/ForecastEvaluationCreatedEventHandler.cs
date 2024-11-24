using MediatR;
using Prediction.Domain.Events;

namespace Prediction.Application.Features.ForecastEvaluations.EventHandlers
{
    public class ForecastEvaluationCreatedEventHandler(IApplicationDbContext context, ILogger<ForecastEvaluationCreatedEventHandler> logger) : INotificationHandler<ForecastEvaluationCreatedEvent>
    {
        public async Task Handle(ForecastEvaluationCreatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("ForecastEvaluationCreatedEventHandler triggered for MatchId: {MatchId}", notification.MatchId);

            // Fetch the match
            var match = await context.Matches.FindAsync(new object[] { notification.MatchId }, cancellationToken);
            if (match == null)
            {
                logger.LogWarning("Match with ID {MatchId} not found.", notification.MatchId);
                throw new ObjectNotFoundException(notification.MatchId.Value, "Match not found.");
            }

            // Fetch the forecast for this match
            var forecast = await context.Forecasts
                .FirstOrDefaultAsync(f => f.MatchId == match.Id, cancellationToken);

            if (forecast == null)
            {
                logger.LogWarning("No forecast found for MatchId: {MatchId}. Skipping ForecastEvaluation.", notification.MatchId);
                return;
            }

            // Determine the actual outcome for the match
            var actualOutcomeIds = await match.GetApplicableOutcomesAsync(context, match.Team1Id, cancellationToken);

            // Compare forecasted outcome with actual outcome
            var wasCorrect = actualOutcomeIds.Contains(forecast.OutcomeTypeId);

            // Create the ForecastEvaluation
            var forecastEvaluation = ForecastEvaluation.Create(
                ForecastEvaluationId.Of(Guid.NewGuid()),
                forecast.Id,
                actualOutcomeIds.First(), // Assume one primary outcome
                forecast.Confidence,
                wasCorrect
            );

            context.ForecastEvaluations.Add(forecastEvaluation);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("ForecastEvaluation created for MatchId: {MatchId} with ForecastId: {ForecastId}. WasCorrect: {WasCorrect}.",
                match.Id.Value, forecast.Id.Value, wasCorrect);
        }
    }
}
