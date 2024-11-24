namespace Prediction.Application.Features.Teams.Commands.UpdateTeam
{
    public class UpdateTeamHandler(IApplicationDbContext context, ILogger<UpdateTeamHandler> logger) : ICommandHandler<UpdateTeamCommand, UpdateTeamResult>
    {
        public async Task<UpdateTeamResult> Handle(UpdateTeamCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateTeamHandler.Handle called with {@Command}", command);

            //Update Team entity from command object
            var teamId = TeamId.Of(command.Team.Id);

            var team = await context.Teams
                .FindAsync(new object[] { teamId }, cancellationToken: cancellationToken);

            if (team is null)
            {
                throw new ObjectNotFoundException(command.Team.Id);
            }

            var teamExists = await context.Teams
                .AnyAsync(t => t.Name == command.Team.Name && t.LeagueId == LeagueId.Of(command.Team.LeagueId), cancellationToken);

            if (teamExists)
            {
                throw new AlreadyExistsException($"Name '{command.Team.Name}' already exists for this League.");
            }

            // save to database
            UpdateTeamWithNewValues(team, command.Team);
            context.Teams.Update(team);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new UpdateTeamResult(true);
        }

        private void UpdateTeamWithNewValues(Team team, TeamDto teamDto)
        {
            // Update properties
            team.Update(
                name: teamDto.Name,
                leagueId: LeagueId.Of(teamDto.LeagueId)
            );
        }
    }
}
