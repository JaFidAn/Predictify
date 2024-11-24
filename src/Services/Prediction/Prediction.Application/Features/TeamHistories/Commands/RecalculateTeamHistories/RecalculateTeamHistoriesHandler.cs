namespace Prediction.Application.Features.TeamHistories.Commands.RecalculateTeamHistories
{
    public class RecalculateTeamHistoriesAndStreaksHandler(IApplicationDbContext context, ILogger<RecalculateTeamHistoriesAndStreaksHandler> logger) : ICommandHandler<RecalculateTeamHistoriesAndStreaksCommand, RecalculateTeamHistoriesAndStreaksResult>
    {
        public async Task<RecalculateTeamHistoriesAndStreaksResult> Handle(RecalculateTeamHistoriesAndStreaksCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("RecalculateTeamHistoriesAndStreaksHandler triggered for all teams.");

            // Fetch all teams
            var teams = await context.Teams.ToListAsync(cancellationToken);

            if (!teams.Any())
            {
                logger.LogWarning("No teams found for recalculating team histories and streaks.");
                return new RecalculateTeamHistoriesAndStreaksResult("No teams found.", 0);
            }

            int updatedTeamsCount = 0;

            foreach (var team in teams)
            {
                // Use the extension method to update team histories for each team
                await team.Id.UpdateTeamHistoriesAsync(context, cancellationToken);
                logger.LogInformation("Team histories recalculated for TeamId: {TeamId}", team.Id.Value);

                // Use the extension method to update streaks for the team
                await team.Id.UpdateStreaksAsync(context, cancellationToken);
                logger.LogInformation("Streaks recalculated for TeamId: {TeamId}", team.Id.Value);

                updatedTeamsCount++;
            }

            logger.LogInformation("Team histories and streaks successfully recalculated for {Count} teams.", updatedTeamsCount);

            return new RecalculateTeamHistoriesAndStreaksResult("Team histories and streaks recalculated successfully.", updatedTeamsCount);
        }
    }
}
