namespace Prediction.Application.Features.Forecasts.Queries.GetForecasts
{
    public record GetForecastsQuery(PaginationRequest PaginationRequest) : IQuery<GetForecastsResult>;
    public record GetForecastsResult(PaginatedResult<ForecastDto> Forecasts);
}
