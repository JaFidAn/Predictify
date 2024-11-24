namespace Prediction.Application.Features.TeamLeagueSeasons.Commands.UpdateTeamLeagueSeason
{
    public record UpdateTeamLeagueSeasonCommand(TeamLeagueSeasonDto TeamLeagueSeason) : ICommand<UpdateTeamLeagueSeasonResult>;
    public record UpdateTeamLeagueSeasonResult(bool IsSuccess);

    public class UpdateTeamLeagueSeasonCommandValidator : AbstractValidator<UpdateTeamLeagueSeasonCommand>
    {
        public UpdateTeamLeagueSeasonCommandValidator()
        {
            RuleFor(x => x.TeamLeagueSeason.Id)
                .NotEmpty()
                .WithMessage("TeamLeagueSeason Id is required.")
                .Must(id => id != Guid.Empty)
                .WithMessage("TeamLeagueSeason Id cannot be empty.");

            RuleFor(x => x.TeamLeagueSeason.TeamId)
                .NotEmpty()
                .WithMessage("TeamId is required.")
                .Must(teamId => teamId != Guid.Empty)
                .WithMessage("TeamId cannot be empty.");

            RuleFor(x => x.TeamLeagueSeason.LeagueId)
                .NotEmpty()
                .WithMessage("LeagueId is required.")
                .Must(leagueId => leagueId != Guid.Empty)
                .WithMessage("LeagueId cannot be empty.");

            RuleFor(x => x.TeamLeagueSeason.SeasonId)
                .NotEmpty()
                .WithMessage("SeasonId is required.")
                .Must(seasonId => seasonId != Guid.Empty)
                .WithMessage("SeasonId cannot be empty.");
        }
    }
}
