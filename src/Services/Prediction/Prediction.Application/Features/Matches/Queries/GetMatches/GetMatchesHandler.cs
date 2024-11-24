namespace Prediction.Application.Features.Matches.Queries.GetMatches
{
    public class GetMatchesHandler(IApplicationDbContext context, ILogger<GetMatchesHandler> logger) : IQueryHandler<GetMatchesQuery, GetMatchesResult>
    {
        public async Task<GetMatchesResult> Handle(GetMatchesQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetLeaguesHandler.Handle called with {@Query}", query);

            //get matches with Pagination
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await context.Matches.LongCountAsync(cancellationToken);

            var matches = await context.Matches
                .ToMatchDtoList(context.Teams, context.OutcomeTypes, context.MatchOutcomeTypes)
                .OrderByDescending(dto => dto.Date)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            //return result
            return new GetMatchesResult(
                new PaginatedResult<MatchDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    matches));
        }
    }
}
