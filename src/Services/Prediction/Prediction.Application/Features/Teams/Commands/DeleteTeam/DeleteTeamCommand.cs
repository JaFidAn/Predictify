namespace Prediction.Application.Features.Teams.Commands.DeleteTeam
{
    public record DeleteTeamCommand(Guid TeamId) : ICommand<DeleteTeamResult>;
    public record DeleteTeamResult(bool IsSuccess);

    public class DeleteTeamCommandValidator : AbstractValidator<DeleteTeamCommand>
    {
        public DeleteTeamCommandValidator()
        {
            RuleFor(x => x.TeamId)
            .NotEmpty()
            .WithMessage("Team Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Team Id cannot be empty.");
        }
    }
}
