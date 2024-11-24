namespace Prediction.Application.Features.Teams.Commands.CreateTeam
{
    public record CreateTeamCommand(TeamDto Team) : ICommand<CreateTeamResult>;
    public record CreateTeamResult(Guid Id);

    public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
    {
        public CreateTeamCommandValidator()
        {
            RuleFor(x => x.Team.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Team name is required.")
                .MaximumLength(50)
                .WithMessage("Team name cannot exceed 50 characters.");

            RuleFor(x => x.Team.LeagueId)
                .NotEmpty()
                .WithMessage("LeagueId is required.")
                .Must(leagueId => leagueId != Guid.Empty)
                .WithMessage("LeagueId cannot be empty.");
        }
    }
}
