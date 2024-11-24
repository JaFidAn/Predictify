namespace Prediction.Application.Features.TeamLeagueSeasons.Queries.GetTeamLeagueSeasonById
{
    public class GetTeamLeagueSeasonByIdHandler(IApplicationDbContext context, ILogger<GetTeamLeagueSeasonByIdHandler> logger) : IQueryHandler<GetTeamLeagueSeasonByIdQuery, GetTeamLeagueSeasonByIdResult>
    {
        public async Task<GetTeamLeagueSeasonByIdResult> Handle(GetTeamLeagueSeasonByIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetTeamLeagueSeasonByIdHandler.Handle called with {@Query}", query);

            //get TeamLeagueSeason by id using context
            var teamLeagueSeason = await context.TeamLeagueSeasons
                .AsNoTracking()
                .FirstOrDefaultAsync(tls => tls.Id == TeamLeagueSeasonId.Of(query.Id), cancellationToken);

            if (teamLeagueSeason is null)
            {
                throw new ObjectNotFoundException(query.Id);
            }

            //return result
            return new GetTeamLeagueSeasonByIdResult(teamLeagueSeason.ToTeamLeagueSeasonDto(context.Teams, context.Leagues, context.Seasons));
        }
    }
}
