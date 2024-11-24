namespace Prediction.Application.Features.Teams.Commands.UpdateTeam
{
    public record UpdateTeamCommand(TeamDto Team) : ICommand<UpdateTeamResult>;
    public record UpdateTeamResult(bool IsSuccess);

    public class UpdateTeamCommandValidator : AbstractValidator<UpdateTeamCommand>
    {
        public UpdateTeamCommandValidator()
        {
            RuleFor(x => x.Team.Id)
            .NotEmpty()
            .WithMessage("Team Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Team Id cannot be empty.");

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
