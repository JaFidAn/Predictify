namespace Prediction.Application.Features.Countries.Commands.DeleteCountry
{
    public class DeleteCountryHandler(IApplicationDbContext context, ILogger<DeleteCountryHandler> logger) : ICommandHandler<DeleteCountryCommand, DeleteCountryResult>
    {
        public async Task<DeleteCountryResult> Handle(DeleteCountryCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteCountryHandler.Handle called with {@Command}", command);

            //Delete Country entity from command object
            var countryId = CountryId.Of(command.CountryId);
            var country = await context.Countries
                .FindAsync(new object[] { countryId }, cancellationToken: cancellationToken);

            if (country is null)
            {
                throw new ObjectNotFoundException(command.CountryId);
            }

            //save to database
            context.Countries.Remove(country);
            await context.SaveChangesAsync(cancellationToken);

            //return result
            return new DeleteCountryResult(true);
        }
    }
}
