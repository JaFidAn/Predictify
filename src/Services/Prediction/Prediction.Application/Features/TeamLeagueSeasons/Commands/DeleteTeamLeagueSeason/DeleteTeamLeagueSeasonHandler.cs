namespace Prediction.Application.Features.TeamLeagueSeasons.Commands.DeleteTeamLeagueSeason
{
    public class DeleteTeamLeagueSeasonHandler(IApplicationDbContext context, ILogger<DeleteTeamLeagueSeasonHandler> logger) : ICommandHandler<DeleteTeamLeagueSeasonCommand, DeleteTeamLeagueSeasonResult>
    {
        public async Task<DeleteTeamLeagueSeasonResult> Handle(DeleteTeamLeagueSeasonCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteTeamLeagueSeasonHandler.Handle called with {@Command}", command);

            //Delete TeamLeagueSeason entity from command object
            var teamLeagueSeasonId = TeamLeagueSeasonId.Of(command.TeamLeagueSeasonId);

            var teamLeagueSeason = await context.TeamLeagueSeasons
                .FindAsync(new object[] { teamLeagueSeasonId }, cancellationToken: cancellationToken);

            if (teamLeagueSeason is null)
            {
                throw new ObjectNotFoundException(command.TeamLeagueSeasonId);
            }

            //save to database
            context.TeamLeagueSeasons.Remove(teamLeagueSeason);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new DeleteTeamLeagueSeasonResult(true);
        }
    }
}
