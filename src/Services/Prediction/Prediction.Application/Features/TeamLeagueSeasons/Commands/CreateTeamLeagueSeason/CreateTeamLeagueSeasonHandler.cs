namespace Prediction.Application.Features.TeamLeagueSeasons.Commands.CreateTeamLeagueSeason
{
    public class CreateTeamLeagueSeasonHandler(IApplicationDbContext context, ILogger<CreateTeamLeagueSeasonHandler> logger) : ICommandHandler<CreateTeamLeagueSeasonCommand, CreateTeamLeagueSeasonResult>
    {
        public async Task<CreateTeamLeagueSeasonResult> Handle(CreateTeamLeagueSeasonCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("CreateTeamLeagueSeasonHandler.Handle called with {@Command}", command);

            var teamLeagueSeasonExists = await context.TeamLeagueSeasons
                .AnyAsync(tls => tls.TeamId == TeamId.Of(command.TeamLeagueSeason.TeamId) && tls.LeagueId == LeagueId.Of(command.TeamLeagueSeason.LeagueId) && tls.SeasonId == SeasonId.Of(command.TeamLeagueSeason.SeasonId), cancellationToken);

            if (teamLeagueSeasonExists)
            {
                throw new AlreadyExistsException($"Name '{command.TeamLeagueSeason.TeamId}' already exists for this Season.");
            }

            //create TeamLeagueSeason entity from command object
            var teamLeagueSeason = CreateNewTeamLeagueSeason(command.TeamLeagueSeason);

            //save to database
            await context.TeamLeagueSeasons.AddAsync(teamLeagueSeason);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new CreateTeamLeagueSeasonResult(teamLeagueSeason.Id.Value);
        }

        private TeamLeagueSeason CreateNewTeamLeagueSeason(TeamLeagueSeasonDto teamLeagueSeasonDto)
        {
            var newTeamLeagueSeason = TeamLeagueSeason.Create(
                id: TeamLeagueSeasonId.Of(Guid.NewGuid()),
                teamId: TeamId.Of(teamLeagueSeasonDto.TeamId),
                leagueId: LeagueId.Of(teamLeagueSeasonDto.LeagueId),
                seasonId: SeasonId.Of(teamLeagueSeasonDto.SeasonId)
                );
            return newTeamLeagueSeason;
        }
    }
}
