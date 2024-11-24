namespace Prediction.Application.Features.OutcomeTypes.Queries.GetOutcomeTypes
{
    public record GetOutcomeTypesQuery(PaginationRequest PaginationRequest) : IQuery<GetOutcomeTypesResult>;
    public record GetOutcomeTypesResult(PaginatedResult<OutcomeTypeDto> OutcomeTypes);
}
