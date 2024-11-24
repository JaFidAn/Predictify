namespace Prediction.Application.Features.Matches.Commands.DeleteMatch
{
    public class DeleteMatchHandler(IApplicationDbContext context, ILogger<DeleteMatchHandler> logger) : ICommandHandler<DeleteMatchCommand, DeleteMatchResult>
    {
        public async Task<DeleteMatchResult> Handle(DeleteMatchCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteMatchHandler.Handle called with {@Command}", command);

            // Fetch the match to delete
            var matchId = MatchId.Of(command.MatchId);
            var match = await context.Matches
                .FindAsync(new object[] { matchId }, cancellationToken);

            if (match is null)
            {
                throw new ObjectNotFoundException(command.MatchId);
            }

            // Delete associated MatchOutcomeTypes
            await match.DeleteMatchOutcomeTypesAsync(context, cancellationToken);

            // Trigger domain event for match deletion
            match.Delete();

            // Remove the match itself
            context.Matches.Remove(match);

            // Save changes to the database
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Match with ID {MatchId} deleted successfully.", matchId.Value);

            return new DeleteMatchResult(true);
        }
    }
}
