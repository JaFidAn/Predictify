namespace Prediction.Application.Features.ForecastEvaluations.Queries.GetForecastEvaluations
{
    public record GetForecastEvaluationsQuery(PaginationRequest PaginationRequest) : IQuery<GetForecastEvaluationsResult>;
    public record GetForecastEvaluationsResult(PaginatedResult<ForecastEvaluationDto> Evaluations);
}
