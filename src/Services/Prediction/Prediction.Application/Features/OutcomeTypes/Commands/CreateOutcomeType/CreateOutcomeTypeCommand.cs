namespace Prediction.Application.Features.OutcomeTypes.Commands.CreateOutcomeType
{
    public record CreateOutcomeTypeCommand(OutcomeTypeDto OutcomeType) : ICommand<CreateOutcomeTypeResult>;
    public record CreateOutcomeTypeResult(Guid Id);

    public class CreateOutcomeTypeCommandValidator : AbstractValidator<CreateOutcomeTypeCommand>
    {
        public CreateOutcomeTypeCommandValidator()
        {
            RuleFor(x => x.OutcomeType.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("OutcomeType name is required.")
            .MaximumLength(20)
            .WithMessage("OutcomeType name cannot exceed 20 characters.");
        }
    }
}
