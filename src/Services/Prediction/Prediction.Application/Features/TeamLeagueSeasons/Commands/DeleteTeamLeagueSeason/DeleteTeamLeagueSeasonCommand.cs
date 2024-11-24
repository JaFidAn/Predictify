namespace Prediction.Application.Features.TeamLeagueSeasons.Commands.DeleteTeamLeagueSeason
{
    public record DeleteTeamLeagueSeasonCommand(Guid TeamLeagueSeasonId) : ICommand<DeleteTeamLeagueSeasonResult>;
    public record DeleteTeamLeagueSeasonResult(bool IsSuccess);

    public class DeleteTeamLeagueSeasonCommandValidator : AbstractValidator<DeleteTeamLeagueSeasonCommand>
    {
        public DeleteTeamLeagueSeasonCommandValidator()
        {
            RuleFor(x => x.TeamLeagueSeasonId)
            .NotEmpty()
            .WithMessage("TeamLeagueSeason Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("TeamLeagueSeason Id cannot be empty.");
        }
    }
}
