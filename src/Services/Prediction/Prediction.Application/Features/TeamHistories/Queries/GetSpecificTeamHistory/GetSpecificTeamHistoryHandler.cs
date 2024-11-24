namespace Prediction.Application.Features.TeamHistories.Queries.GetSpecificTeamHistory
{
    public class GetSpecificTeamHistoryHandler(IApplicationDbContext context, ILogger<GetSpecificTeamHistoryHandler> logger) : IQueryHandler<GetSpecificTeamHistoryQuery, GetSpecificTeamHistoryResult>
    {
        public async Task<GetSpecificTeamHistoryResult> Handle(GetSpecificTeamHistoryQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetSpecificTeamHistoryHandler.Handle called with {@Query}", query);

            // Fetch the specific team history record
            var teamHistoryId = TeamHistoryId.Of(query.TeamHistoryId);
            var teamHistoryQuery = context.TeamHistories
                .Where(th => th.Id == teamHistoryId)
                .ToTeamHistoryDtoList(context.Teams, context.OutcomeTypes); // Using the extension method

            var teamHistory = await teamHistoryQuery.FirstOrDefaultAsync(cancellationToken);

            if (teamHistory == null)
            {
                throw new ObjectNotFoundException(query.TeamHistoryId, "TeamHistory not found.");
            }

            return new GetSpecificTeamHistoryResult(teamHistory);
        }
    }
}
