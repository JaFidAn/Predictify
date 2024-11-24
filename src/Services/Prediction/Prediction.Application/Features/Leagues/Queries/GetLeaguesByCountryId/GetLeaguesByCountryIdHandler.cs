namespace Prediction.Application.Features.Leagues.Queries.GetLeaguesByCountryId
{
    public class GetLeaguesByCountryIdHandler(IApplicationDbContext context, ILogger<GetLeaguesByCountryIdHandler> logger) : IQueryHandler<GetLeaguesByCountryIdQuery, GetLeaguesByCountryIdResult>
    {
        public async Task<GetLeaguesByCountryIdResult> Handle(GetLeaguesByCountryIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetLeaguesByCountryIdHandler.Handle called with {@Query}", query);

            //get leagues by CountryId with Pagination
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await context.Leagues
                .Where(l => l.CountryId == CountryId.Of(query.CountryId))
                .LongCountAsync(cancellationToken);

            var leagues = await context.Leagues
                .Where(l => l.CountryId == CountryId.Of(query.CountryId))
                .ToLeagueDtoList(context.Countries)
                .OrderBy(dto => dto.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            //return result
            return new GetLeaguesByCountryIdResult(
                new PaginatedResult<LeagueDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    leagues));
        }
    }
}
