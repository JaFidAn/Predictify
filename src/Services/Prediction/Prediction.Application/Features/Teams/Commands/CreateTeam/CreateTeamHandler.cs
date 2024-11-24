namespace Prediction.Application.Features.Teams.Commands.CreateTeam
{
    public class CreateTeamHandler(IApplicationDbContext context, ILogger<CreateTeamHandler> logger) : ICommandHandler<CreateTeamCommand, CreateTeamResult>
    {
        public async Task<CreateTeamResult> Handle(CreateTeamCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("CreateTeamHandler.Handle called with {@Command}", command);

            var teamExists = await context.Teams
                .AnyAsync(t => t.Name == command.Team.Name && t.LeagueId == LeagueId.Of(command.Team.LeagueId), cancellationToken);

            if (teamExists)
            {
                throw new AlreadyExistsException($"Name '{command.Team.Name}' already exists for this League.");
            }

            //create Team entity from command object
            var team = CreateNewTeam(command.Team);

            //save to database
            await context.Teams.AddAsync(team);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new CreateTeamResult(team.Id.Value);
        }

        private Team CreateNewTeam(TeamDto teamDto)
        {
            var newTeam = Team.Create(
                id: TeamId.Of(Guid.NewGuid()),
                name: teamDto.Name,
                leagueId: LeagueId.Of(teamDto.LeagueId)
                );
            return newTeam;
        }
    }
}
