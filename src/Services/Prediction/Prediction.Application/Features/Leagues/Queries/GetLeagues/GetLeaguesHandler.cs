namespace Prediction.Application.Features.Leagues.Queries.GetLeagues
{
    public class GetLeaguesHandler(IApplicationDbContext context, ILogger<GetLeaguesHandler> logger) : IQueryHandler<GetLeaguesQuery, GetLeaguesResult>
    {
        public async Task<GetLeaguesResult> Handle(GetLeaguesQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetLeaguesHandler.Handle called with {@Query}", query);

            //get leagues with Pagination
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await context.Leagues.LongCountAsync(cancellationToken);

            var leagues = await context.Leagues
                .ToLeagueDtoList(context.Countries)
                .OrderBy(dto => dto.Country.Name)
                .ThenBy(dto => dto.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            //return result
            return new GetLeaguesResult(
                new PaginatedResult<LeagueDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    leagues));
        }
    }
}
