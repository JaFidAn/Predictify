namespace Prediction.Application.Features.Countries.Commands.CreateCountry
{
    public record CreateCountryCommand(CountryDto Country) : ICommand<CreateCountryResult>;
    public record CreateCountryResult(Guid Id);

    public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
    {
        public CreateCountryCommandValidator()
        {
            RuleFor(x => x.Country.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Country name is required.")
            .MaximumLength(50)
            .WithMessage("Country name cannot exceed 50 characters.");
        }
    }

}
