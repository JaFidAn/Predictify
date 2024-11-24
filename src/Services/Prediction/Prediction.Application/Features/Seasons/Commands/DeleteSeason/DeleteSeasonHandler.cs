namespace Prediction.Application.Features.Seasons.Commands.DeleteSeason
{
    public class DeleteSeasonHandler(IApplicationDbContext context, ILogger<DeleteSeasonHandler> logger) : ICommandHandler<DeleteSeasonCommand, DeleteSeasonResult>
    {
        public async Task<DeleteSeasonResult> Handle(DeleteSeasonCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteSeasonHandler.Handle called with {@Command}", command);

            //Delete Season entity from command object
            var seasonId = SeasonId.Of(command.SeasonId);

            var season = await context.Seasons
                .FindAsync(new object[] { seasonId }, cancellationToken: cancellationToken);

            if (season is null)
            {
                throw new ObjectNotFoundException(command.SeasonId);
            }

            //save to database
            context.Seasons.Remove(season);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new DeleteSeasonResult(true);
        }
    }
}
