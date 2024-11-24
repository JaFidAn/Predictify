namespace Prediction.Application.Features.OutcomeTypes.Commands.CreateOutcomeType
{
    public class CreateOutcomeTypeHandler(IApplicationDbContext context, ILogger<CreateOutcomeTypeHandler> logger) : ICommandHandler<CreateOutcomeTypeCommand, CreateOutcomeTypeResult>
    {
        public async Task<CreateOutcomeTypeResult> Handle(CreateOutcomeTypeCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("CreateOutcomeTypeHandler.Handle called with {@Command}", command);

            var outcomeTypeExists = await context.OutcomeTypes
                .AnyAsync(c => c.Name == command.OutcomeType.Name);

            if (outcomeTypeExists)
            {
                throw new AlreadyExistsException($"Name '{command.OutcomeType.Name}' already exists.");
            }

            //create OutcomeType entity from command object
            var outcomeType = CreateNewOutcomeType(command.OutcomeType);

            //save to database
            await context.OutcomeTypes.AddAsync(outcomeType);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new CreateOutcomeTypeResult(outcomeType.Id.Value);
        }

        private OutcomeType CreateNewOutcomeType(OutcomeTypeDto outcomeTypeDto)
        {
            var newOutcomeType = OutcomeType.Create(
                id: OutcomeTypeId.Of(Guid.NewGuid()),
                name: outcomeTypeDto.Name,
                description: outcomeTypeDto.Description
                );
            return newOutcomeType;
        }
    }
}
