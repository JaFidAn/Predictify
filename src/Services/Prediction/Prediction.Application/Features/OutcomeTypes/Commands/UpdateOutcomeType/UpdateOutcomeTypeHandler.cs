namespace Prediction.Application.Features.OutcomeTypes.Commands.UpdateOutcomeType
{
    public class UpdateOutcomeTypeHandler(IApplicationDbContext context, ILogger<UpdateOutcomeTypeHandler> logger) : ICommandHandler<UpdateOutcomeTypeCommand, UpdateOutcomeTypeResult>
    {
        public async Task<UpdateOutcomeTypeResult> Handle(UpdateOutcomeTypeCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateOutcomeTypeHandler.Handle called with {@Command}", command);

            //Update OutcomeType entity from command object
            var outcomeTypeId = OutcomeTypeId.Of(command.OutcomeType.Id);

            var outcomeType = await context.OutcomeTypes
                .FindAsync(new object[] { outcomeTypeId }, cancellationToken: cancellationToken);

            if (outcomeType is null)
            {
                throw new ObjectNotFoundException(command.OutcomeType.Id);
            }

            bool outcomeTypeExists = await context.OutcomeTypes
                .AnyAsync(ot => ot.Name == command.OutcomeType.Name);

            if (outcomeTypeExists)
            {
                throw new AlreadyExistsException($"Name '{command.OutcomeType.Name}' already exists.");
            }

            // save to database
            UpdateOutcomeTypeWithNewValues(outcomeType, command.OutcomeType);
            context.OutcomeTypes.Update(outcomeType);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new UpdateOutcomeTypeResult(true);
        }

        private void UpdateOutcomeTypeWithNewValues(OutcomeType outcomeType, OutcomeTypeDto outcomeTypeDto)
        {
            // Update properties
            outcomeType.Update(
                name: outcomeTypeDto.Name,
                description: outcomeTypeDto.Description
            );
        }
    }
}
