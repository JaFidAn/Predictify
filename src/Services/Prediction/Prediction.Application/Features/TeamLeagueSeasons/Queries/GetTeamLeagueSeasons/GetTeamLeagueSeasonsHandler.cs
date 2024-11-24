namespace Prediction.Application.Features.TeamLeagueSeasons.Queries.GetTeamLeagueSeasons
{
    public class GetTeamLeagueSeasonsHandler(IApplicationDbContext context, ILogger<GetTeamLeagueSeasonsHandler> logger) : IQueryHandler<GetTeamLeagueSeasonsQuery, GetTeamLeagueSeasonsResult>
    {
        public async Task<GetTeamLeagueSeasonsResult> Handle(GetTeamLeagueSeasonsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetTeamLeagueSeasonsHandler.Handle called with {@Query}", query);

            //get TeamLeagueSeasons with Pagination
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await context.TeamLeagueSeasons.LongCountAsync(cancellationToken);

            var teamLeagueSeasons = await context.TeamLeagueSeasons
                .ToTeamLeagueSeasonDtoList(context.Teams, context.Leagues, context.Seasons)
                .OrderByDescending(dto => dto.SeasonName)
                .ThenBy(dto => dto.LeagueName)
                .ThenBy(dto => dto.TeamName)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            //return result
            return new GetTeamLeagueSeasonsResult(
                new PaginatedResult<TeamLeagueSeasonDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    teamLeagueSeasons));
        }
    }
}
