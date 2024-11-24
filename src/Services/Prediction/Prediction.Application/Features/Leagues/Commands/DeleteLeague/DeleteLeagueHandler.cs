namespace Prediction.Application.Features.Leagues.Commands.DeleteLeague
{
    public class DeleteLeagueHandler(IApplicationDbContext context, ILogger<DeleteLeagueHandler> logger) : ICommandHandler<DeleteLeagueCommand, DeleteLeagueResult>
    {
        public async Task<DeleteLeagueResult> Handle(DeleteLeagueCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteLeagueHandler.Handle called with {@Command}", command);

            //Delete League entity from command object
            var leagueId = LeagueId.Of(command.LeagueId);
            var league = await context.Leagues
                .FindAsync(new object[] { leagueId }, cancellationToken: cancellationToken);

            if (league is null)
            {
                throw new ObjectNotFoundException(command.LeagueId);
            }

            //save to database
            context.Leagues.Remove(league);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new DeleteLeagueResult(true);
        }
    }
}
