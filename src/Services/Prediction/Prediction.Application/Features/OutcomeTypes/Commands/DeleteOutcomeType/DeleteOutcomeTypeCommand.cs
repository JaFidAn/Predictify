namespace Prediction.Application.Features.OutcomeTypes.Commands.DeleteOutcomeType
{
    public record DeleteOutcomeTypeCommand(Guid OutcomeTypeId) : ICommand<DeleteOutcomeTypeResult>;
    public record DeleteOutcomeTypeResult(bool IsSuccess);

    public class DeleteOutcomeTypeCommandValidator : AbstractValidator<DeleteOutcomeTypeCommand>
    {
        public DeleteOutcomeTypeCommandValidator()
        {
            RuleFor(x => x.OutcomeTypeId)
           .NotEmpty()
           .WithMessage("OutcomeType Id is required.")
           .Must(id => id != Guid.Empty)
           .WithMessage("OutcomeType Id cannot be empty.");
        }
    }
}
