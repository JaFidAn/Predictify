namespace Prediction.Application.Features.OutcomeTypes.Commands.DeleteOutcomeType
{
    public class DeleteOutcomeTypeHandler(IApplicationDbContext context, ILogger<DeleteOutcomeTypeHandler> logger) : ICommandHandler<DeleteOutcomeTypeCommand, DeleteOutcomeTypeResult>
    {
        public async Task<DeleteOutcomeTypeResult> Handle(DeleteOutcomeTypeCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteOutcomeTypeHandler.Handle called with {@Command}", command);

            //Delete OutcomeType entity from command object
            var outcomeTypeId = OutcomeTypeId.Of(command.OutcomeTypeId);

            var outcomeType = await context.OutcomeTypes
                .FindAsync(new object[] { outcomeTypeId }, cancellationToken: cancellationToken);

            if (outcomeType is null)
            {
                throw new ObjectNotFoundException(command.OutcomeTypeId);
            }

            //save to database
            context.OutcomeTypes.Remove(outcomeType);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new DeleteOutcomeTypeResult(true);
        }
    }
}
