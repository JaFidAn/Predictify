namespace Prediction.Application.Features.Leagues.Commands.CreateLeague
{
    public class CreateLeagueHandler(IApplicationDbContext context, ILogger<CreateLeagueHandler> logger) : ICommandHandler<CreateLeagueCommand, CreateLeagueResult>
    {
        public async Task<CreateLeagueResult> Handle(CreateLeagueCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("CreateLeagueHandler.Handle called with {@Command}", command);

            var leagueExists = await context.Leagues
                .AnyAsync(l => l.Name == command.League.Name && l.CountryId == CountryId.Of(command.League.CountryId), cancellationToken);

            if (leagueExists)
            {
                throw new AlreadyExistsException($"Name '{command.League.Name}' already exists for this Country.");
            }

            //create League entity from command object
            var league = CreateNewLeague(command.League);

            //save to database
            await context.Leagues.AddAsync(league);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new CreateLeagueResult(league.Id.Value);
        }

        private League CreateNewLeague(LeagueDto leagueDto)
        {
            var newLeague = League.Create(
                id: LeagueId.Of(Guid.NewGuid()),
                name: leagueDto.Name,
                countryId: CountryId.Of(leagueDto.CountryId)
                );
            return newLeague;
        }
    }
}
