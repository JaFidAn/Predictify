namespace Prediction.Application.Features.Seasons.Commands.UpdateSeason
{
    public class UpdateSeasonHandler(IApplicationDbContext context, ILogger<UpdateSeasonHandler> logger) : ICommandHandler<UpdateSeasonCommand, UpdateSeasonResult>
    {
        public async Task<UpdateSeasonResult> Handle(UpdateSeasonCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateSeasonHandler.Handle called with {@Command}", command);

            //Update Season entity from command object
            var seasonId = SeasonId.Of(command.Season.Id);

            var season = await context.Seasons
                .FindAsync(new object[] { seasonId }, cancellationToken: cancellationToken);

            if (season is null)
            {
                throw new ObjectNotFoundException(command.Season.Id);
            }

            bool seasonExists = await context.Seasons
                .AnyAsync(s => s.Name == command.Season.Name);

            if (seasonExists)
            {
                throw new AlreadyExistsException($"Name '{command.Season.Name}' already exists.");
            }

            // save to database
            UpdateSeasonWithNewValues(season, command.Season);
            context.Seasons.Update(season);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new UpdateSeasonResult(true);
        }

        private void UpdateSeasonWithNewValues(Season season, SeasonDto seasonDto)
        {
            // Update properties
            season.Update(
                name: seasonDto.Name,
                startDate: seasonDto.StartDate,
                endDate: seasonDto.EndDate
            );
        }
    }
}
