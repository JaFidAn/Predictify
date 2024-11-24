namespace Prediction.Application.Features.Seasons.Queries.GetSeasons
{
    public class GetSeasonsHandler(IApplicationDbContext context, ILogger<GetSeasonsHandler> logger) : IQueryHandler<GetSeasonsQuery, GetSeasonsResult>
    {
        public async Task<GetSeasonsResult> Handle(GetSeasonsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetSeasonsHandler.Handle called with {@Query}", query);

            //get seasons with Pagination
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await context.Seasons.LongCountAsync(cancellationToken);

            var seasons = await context.Seasons
                .OrderByDescending(c => c.EndDate)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            //return result
            return new GetSeasonsResult(
                new PaginatedResult<SeasonDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    seasons.ToSeasonDtoList()));
        }
    }
}
