namespace Prediction.Application.Features.OutcomeTypes.Queries.GetOutcomeTypes
{
    public class GetOutcomeTypesHandler(IApplicationDbContext context, ILogger<GetOutcomeTypesHandler> logger) : IQueryHandler<GetOutcomeTypesQuery, GetOutcomeTypesResult>
    {
        public async Task<GetOutcomeTypesResult> Handle(GetOutcomeTypesQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetOutcomeTypesHandler.Handle called with {@Query}", query);

            //get outcomeTypes with Pagination
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await context.OutcomeTypes.LongCountAsync(cancellationToken);

            var outcomeTypes = await context.OutcomeTypes
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            //return result
            return new GetOutcomeTypesResult(
                new PaginatedResult<OutcomeTypeDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    outcomeTypes.ToOutcomeTypeDtoList()));
        }
    }
}
