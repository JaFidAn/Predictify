namespace Prediction.Application.Features.TeamHistories.Queries.GetTeamHistoriesByTeam
{
    public class GetTeamHistoriesByTeamHandler(IApplicationDbContext context, ILogger<GetTeamHistoriesByTeamHandler> logger)
    : IQueryHandler<GetTeamHistoriesByTeamQuery, GetTeamHistoriesByTeamResult>
    {
        public async Task<GetTeamHistoriesByTeamResult> Handle(GetTeamHistoriesByTeamQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetTeamHistoriesByTeamHandler.Handle called with {@Query}", query);

            var teamId = TeamId.Of(query.TeamId);

            // Fetch team histories, teams, and outcome types
            var teamHistoriesQuery = context.TeamHistories
                .Where(th => th.TeamId == teamId)
                .OrderByDescending(th => th.Date);

            var teamHistories = await teamHistoriesQuery
                .ToTeamHistoryDtoList(context.Teams, context.OutcomeTypes)
                .ToListAsync(cancellationToken);

            return new GetTeamHistoriesByTeamResult(teamHistories);
        }
    }
}
