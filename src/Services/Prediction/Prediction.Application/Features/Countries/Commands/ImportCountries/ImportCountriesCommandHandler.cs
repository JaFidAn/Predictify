using Microsoft.EntityFrameworkCore;
using Prediction.Application.Services;

namespace Prediction.Application.Features.Countries.Commands.ImportCountries
{
    public class ImportCountriesCommandHandler(IApplicationDbContext context, ICountryApiService countryApiService, ILogger<ImportCountriesCommandHandler> logger) : ICommandHandler<ImportCountriesCommand, ImportCountriesResult>
    {
        public async Task<ImportCountriesResult> Handle(ImportCountriesCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting the import of countries.");

            // Fetch countries from the API service
            var countryNames = await countryApiService.GetCountryNamesAsync(cancellationToken);

            if (countryNames == null || !countryNames.Any())
            {
                logger.LogWarning("No countries were fetched from the API.");
                return new ImportCountriesResult(false);
            }

            foreach (var countryName in countryNames)
            {
                // Check if the country already exists in the database
                var exists = await context.Countries
                    .AnyAsync(c => c.Name == countryName, cancellationToken);

                if (!exists)
                {
                    // Create a new Country entity
                    var country = Country.Create(CountryId.Of(Guid.NewGuid()), countryName);

                    await context.Countries.AddAsync(country, cancellationToken);
                }
            }

            // Save changes to the database
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully imported countries into the database.");
            return new ImportCountriesResult(true);
        }
    }
}
