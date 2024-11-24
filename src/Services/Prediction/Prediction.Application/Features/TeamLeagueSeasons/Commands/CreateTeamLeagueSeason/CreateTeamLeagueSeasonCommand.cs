namespace Prediction.Application.Features.TeamLeagueSeasons.Commands.CreateTeamLeagueSeason
{
    public record CreateTeamLeagueSeasonCommand(TeamLeagueSeasonDto TeamLeagueSeason) : ICommand<CreateTeamLeagueSeasonResult>;
    public record CreateTeamLeagueSeasonResult(Guid Id);

    public class CreateTeamLeagueSeasonCommandValidator : AbstractValidator<CreateTeamLeagueSeasonCommand>
    {
        public CreateTeamLeagueSeasonCommandValidator()
        {            
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
