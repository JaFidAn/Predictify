using Microsoft.EntityFrameworkCore;

namespace Prediction.Application.Features.Countries.Commands.UpdateCountry
{
    public class UpdateCountryHandler(IApplicationDbContext context, ILogger<UpdateCountryHandler> logger) : ICommandHandler<UpdateCountryCommand, UpdateCountryResult>
    {
        public async Task<UpdateCountryResult> Handle(UpdateCountryCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateCountryHandler.Handle called with {@Command}", command);

            //Update Country entity from command object
            var countryId = CountryId.Of(command.Country.Id);

            var country = await context.Countries
                .FindAsync(new object[] { countryId }, cancellationToken: cancellationToken);

            if (country is null)
            {
                throw new ObjectNotFoundException(command.Country.Id);
            }

            bool countryExists = await context.Countries
                .AnyAsync(c => c.Name == command.Country.Name);

            if (countryExists)
            {
                throw new AlreadyExistsException($"Name '{command.Country.Name}' already exists.");
            }

            // save to database
            UpdateCountryWithNewValues(country, command.Country);
            context.Countries.Update(country);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new UpdateCountryResult(true);
        }

        private void UpdateCountryWithNewValues(Country country, CountryDto countryDto)
        {
            // Update properties
            country.Update(
                name: countryDto.Name
            );
        }
    }
}
