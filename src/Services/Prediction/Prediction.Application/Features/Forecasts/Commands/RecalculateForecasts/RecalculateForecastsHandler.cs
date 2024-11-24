using MediatR;

namespace Prediction.Application.Features.Forecasts.Commands.RecalculateForecasts
{
    public class RecalculateForecastsHandler(IApplicationDbContext context, ILogger<RecalculateForecastsHandler> logger) : IRequestHandler<RecalculateForecastsCommand, RecalculateForecastsResult>
    {
        public async Task<RecalculateForecastsResult> Handle(RecalculateForecastsCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("RecalculateForecastsHandler started. MatchId: {MatchId}", command.MatchId);

            // Fetch matches to recalculate forecasts
            var matchesToProcess = command.MatchId.HasValue
                ? await context.Matches
                    .Where(m => m.Id == MatchId.Of(command.MatchId.Value) && !m.IsCompleted)
                    .ToListAsync(cancellationToken)
                : await context.Matches
                    .Where(m => !m.IsCompleted)
                    .ToListAsync(cancellationToken);

            if (!matchesToProcess.Any())
            {
                logger.LogInformation("No matches found for recalculating forecasts.");
                return new RecalculateForecastsResult(0);
            }

            int forecastsRecalculated = 0;

            foreach (var match in matchesToProcess)
            {
                // Calculate and save forecasts for the match
                await match.CreateForecastForMatchAsync(context, logger, cancellationToken);
                forecastsRecalculated++;
            }

            logger.LogInformation("Recalculation completed. Total forecasts recalculated: {Count}", forecastsRecalculated);
            return new RecalculateForecastsResult(forecastsRecalculated);
        }
    }
}
