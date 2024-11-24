namespace Prediction.Application.Features.OutcomeTypes.Queries.GetOutcomeTypeById
{
    public record GetOutcomeTypeByIdQuery(Guid Id) : IQuery<GetOutcomeTypeByIdResult>;
    public record GetOutcomeTypeByIdResult(OutcomeTypeDto OutcomeType);
}
