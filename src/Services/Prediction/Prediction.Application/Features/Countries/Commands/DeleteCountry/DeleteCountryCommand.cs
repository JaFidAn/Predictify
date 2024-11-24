namespace Prediction.Application.Features.Countries.Commands.DeleteCountry
{
    public record DeleteCountryCommand(Guid CountryId) : ICommand<DeleteCountryResult>;
    public record DeleteCountryResult(bool IsSuccess);

    public class DeleteCountryCommandValidator : AbstractValidator<DeleteCountryCommand>
    {
        public DeleteCountryCommandValidator()
        {
            RuleFor(x => x.CountryId)
           .NotEmpty()
           .WithMessage("Country Id is required.")
           .Must(id => id != Guid.Empty)
           .WithMessage("Country Id cannot be empty.");
        }
    }
}
