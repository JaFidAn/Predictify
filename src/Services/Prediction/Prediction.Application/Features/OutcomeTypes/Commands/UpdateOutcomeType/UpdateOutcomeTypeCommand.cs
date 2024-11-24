namespace Prediction.Application.Features.OutcomeTypes.Commands.UpdateOutcomeType
{
    public record UpdateOutcomeTypeCommand(OutcomeTypeDto OutcomeType) : ICommand<UpdateOutcomeTypeResult>;
    public record UpdateOutcomeTypeResult(bool IsSuccess);

    public class UpdateOutcomeTypeCommandValidator : AbstractValidator<UpdateOutcomeTypeCommand>
    {
        public UpdateOutcomeTypeCommandValidator()
        {
            RuleFor(x => x.OutcomeType.Id)
                .NotEmpty()
                .WithMessage("OutcomeType Id is required.")
                .Must(id => id != Guid.Empty)
                .WithMessage("Country Id cannot be empty.");

            RuleFor(x => x.OutcomeType.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("OutcomeType name is required.")
                .MaximumLength(20)
                .WithMessage("OutcomeType name cannot exceed 20 characters.");
        }
    }
}
