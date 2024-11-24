namespace Prediction.Application.Features.Leagues.Commands.UpdateLeague
{
    public class UpdateLeagueHandler(IApplicationDbContext context, ILogger<UpdateLeagueHandler> logger) : ICommandHandler<UpdateLeagueCommand, UpdateLeagueResult>
    {
        public async Task<UpdateLeagueResult> Handle(UpdateLeagueCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateLeagueHandler.Handle called with {@Command}", command);

            //Update League entity from command object
            var leagueId = LeagueId.Of(command.League.Id);

            var league = await context.Leagues
                .FindAsync(new object[] { leagueId }, cancellationToken: cancellationToken);

            if (league is null)
            {
                throw new ObjectNotFoundException(command.League.Id);
            }

            var leagueExists = await context.Leagues
                .AnyAsync(l => l.Name == command.League.Name && l.CountryId == CountryId.Of(command.League.CountryId), cancellationToken);

            if (leagueExists)
            {
                throw new AlreadyExistsException($"Name '{command.League.Name}' already exists for this Country.");
            }

            // save to database
            UpdateLeagueWithNewValues(league, command.League);
            context.Leagues.Update(league);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new UpdateLeagueResult(true);
        }

        private void UpdateLeagueWithNewValues(League league, LeagueDto leagueDto)
        {
            // Update properties
            league.Update(
                name: leagueDto.Name,
                countryId: CountryId.Of(leagueDto.CountryId)
            );
        }
    }
}
