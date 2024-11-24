namespace Prediction.Application.Features.Teams.Commands.DeleteTeam
{
    public class DeleteTeamHandler(IApplicationDbContext context, ILogger<DeleteTeamHandler> logger) : ICommandHandler<DeleteTeamCommand, DeleteTeamResult>
    {
        public async Task<DeleteTeamResult> Handle(DeleteTeamCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteTeamHandler.Handle called with {@Command}", command);

            //Delete Team entity from command object
            var teamId = TeamId.Of(command.TeamId);

            var team = await context.Teams
                .FindAsync(new object[] { teamId }, cancellationToken: cancellationToken);

            if (team is null)
            {
                throw new ObjectNotFoundException(command.TeamId);
            }

            //save to database
            context.Teams.Remove(team);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new DeleteTeamResult(true);
        }
    }
}
