namespace Prediction.Application.Features.Forecasts.Commands.RecalculateForecasts
{
    public record RecalculateForecastsCommand(Guid? MatchId) : ICommand<RecalculateForecastsResult>;
    public record RecalculateForecastsResult(int ForecastsRecalculated);
}
