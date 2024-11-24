namespace Prediction.Application.Features.Teams.Queries.GetTeamsOfLeagueBySeason
{
    public class GetTeamsOfLeagueBySeasonHandler(IApplicationDbContext context, ILogger<GetTeamsOfLeagueBySeasonHandler> logger)
        : IQueryHandler<GetTeamsOfLeagueBySeasonQuery, GetTeamsOfLeagueBySeasonResult>
    {
        public async Task<GetTeamsOfLeagueBySeasonResult> Handle(GetTeamsOfLeagueBySeasonQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetTeamsOfLeagueBySeasonQuery with LeagueId: {LeagueId} and SeasonId: {SeasonId}", query.LeagueId, query.SeasonId);

            var teams = await context.TeamLeagueSeasons
                .TeamsOfLeagueBySeasonToDto(context.Teams, context.Leagues, context.Seasons, query.LeagueId, query.SeasonId)
                .OrderBy(t => t.TeamName)
                .ToListAsync(cancellationToken);

            return new GetTeamsOfLeagueBySeasonResult(teams);
        }
    }
}
