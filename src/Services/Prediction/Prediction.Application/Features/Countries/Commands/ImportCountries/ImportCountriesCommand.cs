namespace Prediction.Application.Features.Countries.Commands.ImportCountries
{
    public record ImportCountriesCommand() : ICommand<ImportCountriesResult>;
    public record ImportCountriesResult(bool IsImported);
}
