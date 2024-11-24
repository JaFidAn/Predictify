namespace Prediction.Application.Features.Seasons.Commands.CreateSeason
{
    public class CreateSeasonHandler(IApplicationDbContext context, ILogger<CreateSeasonHandler> logger) : ICommandHandler<CreateSeasonCommand, CreateSeasonResult>
    {
        public async Task<CreateSeasonResult> Handle(CreateSeasonCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("CreateSeasonHandler.Handle called with {@Command}", command);

            var seasonExists = await context.Seasons
                .AnyAsync(c => c.Name == command.Season.Name);

            if (seasonExists)
            {
                throw new AlreadyExistsException($"Name '{command.Season.Name}' already exists.");
            }

            //create Season entity from command object
            var season = CreateNewSeason(command.Season);

            //save to database
            await context.Seasons.AddAsync(season);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new CreateSeasonResult(season.Id.Value);
        }

        private Season CreateNewSeason(SeasonDto seasonDto)
        {
            var newSeason = Season.Create(
                id: SeasonId.Of(Guid.NewGuid()),
                name: seasonDto.Name,
                startDate: seasonDto.StartDate,
                endDate: seasonDto.EndDate
                );
            return newSeason;
        }
    }
}
