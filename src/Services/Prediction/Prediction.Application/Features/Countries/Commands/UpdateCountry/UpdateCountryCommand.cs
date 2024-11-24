namespace Prediction.Application.Features.Countries.Commands.UpdateCountry
{
    public record UpdateCountryCommand(CountryDto Country) : ICommand<UpdateCountryResult>;
    public record UpdateCountryResult(bool IsSuccess);

    public class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
    {
        public UpdateCountryCommandValidator()
        {
            RuleFor(x => x.Country.Id)
                .NotEmpty()
                .WithMessage("Country Id is required.")
                .Must(id => id != Guid.Empty)
                .WithMessage("Country Id cannot be empty.");
                        
            RuleFor(x => x.Country.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Country name is required.")
                .MaximumLength(50)
                .WithMessage("Country name cannot exceed 50 characters.");
        }
    }
}
