namespace Prediction.Application.Features.Leagues.Commands.UpdateLeague
{
    public record UpdateLeagueCommand(LeagueDto League) : ICommand<UpdateLeagueResult>;
    public record UpdateLeagueResult(bool IsSuccess);

    public class UpdateLeagueCommandValidator : AbstractValidator<UpdateLeagueCommand>
    {
        public UpdateLeagueCommandValidator()
        {
            RuleFor(x => x.League.Id)
            .NotEmpty()
            .WithMessage("League Id is required.")
            .Must(id => id != Guid.Empty)
            .WithMessage("League Id cannot be empty.");

            RuleFor(x => x.League.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("League name is required.")
                .MaximumLength(50)
                .WithMessage("League name cannot exceed 50 characters.");

            RuleFor(x => x.League.CountryId)
                .NotEmpty()
                .WithMessage("CountryId is required.")
                .Must(countryId => countryId != Guid.Empty)
                .WithMessage("CountryId cannot be empty.");
        }
    }
}
