namespace Prediction.Application.Features.Teams.Queries.GetTeamsByLeagueId
{
    public class GetTeamsByLeagueIdHandler(IApplicationDbContext context, ILogger<GetTeamsByLeagueIdHandler> logger) : IQueryHandler<GetTeamsByLeagueIdQuery, GetTeamsByLeagueIdResult>
    {
        public async Task<GetTeamsByLeagueIdResult> Handle(GetTeamsByLeagueIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetTeamsByLeagueIdHandler.Handle called with {@Query}", query);

            //get teams by LeagueId with Pagination
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await context.Teams
                .Where(t => t.LeagueId == LeagueId.Of(query.LeagueId))
                .LongCountAsync(cancellationToken);

            var teams = (await context.Teams
                .ToTeamDtoListAsync(context.Leagues, context.Countries, cancellationToken))
                .OrderBy(dto => dto.League.Country.Name)
                .ThenBy(dto => dto.League.Name)
                .ThenBy(dto => dto.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToList();

            //return result
            return new GetTeamsByLeagueIdResult(
                new PaginatedResult<TeamDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    teams));
        }
    }
}
