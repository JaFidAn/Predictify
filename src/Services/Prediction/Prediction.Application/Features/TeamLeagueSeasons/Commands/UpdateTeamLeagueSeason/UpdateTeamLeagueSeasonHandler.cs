namespace Prediction.Application.Features.TeamLeagueSeasons.Commands.UpdateTeamLeagueSeason
{
    public class UpdateTeamLeagueSeasonHandler(IApplicationDbContext context, ILogger<UpdateTeamLeagueSeasonHandler> logger) : ICommandHandler<UpdateTeamLeagueSeasonCommand, UpdateTeamLeagueSeasonResult>
    {
        public async Task<UpdateTeamLeagueSeasonResult> Handle(UpdateTeamLeagueSeasonCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateTeamLeagueSeasonHandler.Handle called with {@Command}", command);

            //Update TeamLeagueSeason entity from command object
            var teamLeagueSeasonId = TeamLeagueSeasonId.Of(command.TeamLeagueSeason.Id);

            var teamLeagueSeason = await context.TeamLeagueSeasons
                .FindAsync(new object[] { teamLeagueSeasonId }, cancellationToken: cancellationToken);

            if (teamLeagueSeason is null)
            {
                throw new ObjectNotFoundException(command.TeamLeagueSeason.Id);
            }

            var teamLeagueSeasonExists = await context.TeamLeagueSeasons
                .AnyAsync(tls => tls.TeamId == TeamId.Of(command.TeamLeagueSeason.TeamId) && tls.LeagueId == LeagueId.Of(command.TeamLeagueSeason.LeagueId) && tls.SeasonId == SeasonId.Of(command.TeamLeagueSeason.SeasonId), cancellationToken);

            if (teamLeagueSeasonExists)
            {
                throw new AlreadyExistsException($"Name '{command.TeamLeagueSeason.TeamId}' already exists for this Season.");
            }

            // save to database
            UpdateTeamLeagueSeasonWithNewValues(teamLeagueSeason, command.TeamLeagueSeason);
            context.TeamLeagueSeasons.Update(teamLeagueSeason);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new UpdateTeamLeagueSeasonResult(true);
        }

        private void UpdateTeamLeagueSeasonWithNewValues(TeamLeagueSeason teamLeagueSeason, TeamLeagueSeasonDto teamLeagueSeasonDto)
        {
            // Update properties
            teamLeagueSeason.Update(
                teamId: TeamId.Of(teamLeagueSeasonDto.TeamId),
                leagueId: LeagueId.Of(teamLeagueSeasonDto.LeagueId),
                seasonId: SeasonId.Of(teamLeagueSeasonDto.SeasonId)
            );
        }
    }
}
