namespace Prediction.Application.Features.OutcomeTypes.Queries.GetOutcomeTypeById
{
    public class GetOutcomeTypeByIdHandler(IApplicationDbContext context, ILogger<GetOutcomeTypeByIdHandler> logger) : IQueryHandler<GetOutcomeTypeByIdQuery, GetOutcomeTypeByIdResult>
    {
        public async Task<GetOutcomeTypeByIdResult> Handle(GetOutcomeTypeByIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetOutcomeTypeByIdHandler.Handle called with {@Query}", query);

            //get outcomeType by id using context
            var outcomeType = await context.OutcomeTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(ot => ot.Id == OutcomeTypeId.Of(query.Id), cancellationToken);

            if (outcomeType is null)
            {
                throw new ObjectNotFoundException(query.Id);
            }

            //return result
            return new GetOutcomeTypeByIdResult(outcomeType.ToOutcomeTypeDto());
        }
    }
}
