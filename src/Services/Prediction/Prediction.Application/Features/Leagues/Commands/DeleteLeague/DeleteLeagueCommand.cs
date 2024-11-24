namespace Prediction.Application.Features.Leagues.Commands.DeleteLeague
{
    public record DeleteLeagueCommand(Guid LeagueId) : ICommand<DeleteLeagueResult>;
    public record DeleteLeagueResult(bool IsSuccess);

    public class DeleteLeagueCommandValidator : AbstractValidator<DeleteLeagueCommand>
    {
        public DeleteLeagueCommandValidator()
        {
            RuleFor(x => x.LeagueId)
            .NotEmpty()
            .WithMessage("League Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("League Id cannot be empty.");
        }
    }
}
