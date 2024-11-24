namespace Prediction.Application.Features.Countries.Commands.CreateCountry
{
    public class CreateCountryHandler(IApplicationDbContext context, ILogger<CreateCountryHandler> logger) : ICommandHandler<CreateCountryCommand, CreateCountryResult>
    {
        public async Task<CreateCountryResult> Handle(CreateCountryCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("CreateCountryHandler.Handle called with {@Command}", command);

            var countryExists = await context.Countries
                .AnyAsync(c => c.Name == command.Country.Name);

            if (countryExists)
            {
                throw new AlreadyExistsException($"Name '{command.Country.Name}' already exists.");
            }

            //create Country entity from command object
            var country = CreateNewCountry(command.Country);

            //save to database
            await context.Countries.AddAsync(country);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new CreateCountryResult(country.Id.Value);
        }

        private Country CreateNewCountry(CountryDto countryDto)
        {
            var newCountry = Country.Create(
                id: CountryId.Of(Guid.NewGuid()),
                name: countryDto.Name
                );
            return newCountry;
        }
    }
}
