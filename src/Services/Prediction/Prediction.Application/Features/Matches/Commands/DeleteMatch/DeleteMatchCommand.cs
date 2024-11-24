namespace Prediction.Application.Features.Matches.Commands.DeleteMatch
{
    public record DeleteMatchCommand(Guid MatchId) : ICommand<DeleteMatchResult>;
    public record DeleteMatchResult(bool IsSuccess);

    public class DeleteMatchCommandValidator : AbstractValidator<DeleteMatchCommand>
    {
        public DeleteMatchCommandValidator()
        {
            RuleFor(x => x.MatchId)
            .NotEmpty()
            .WithMessage("Match Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Match Id cannot be empty.");
        }
    }

}
