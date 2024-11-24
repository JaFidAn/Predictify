namespace Prediction.Application.Features.Leagues.Commands.CreateLeague
{
    public record CreateLeagueCommand(LeagueDto League) : ICommand<CreateLeagueResult>;
    public record CreateLeagueResult(Guid Id);

    public class CreateLeagueCommandValidator : AbstractValidator<CreateLeagueCommand>
    {
        public CreateLeagueCommandValidator()
        {
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
