namespace Prediction.Application.Features.Countries.Queries.GetCountryById
{
    public class GetCountryByIdHandler(IApplicationDbContext context, ILogger<GetCountryByIdHandler> logger) : IQueryHandler<GetCountryByIdQuery, GetCountryByIdResult>
    {
        public async Task<GetCountryByIdResult> Handle(GetCountryByIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetCountryByIdHandler.Handle called with {@Query}", query);

            //get country by id using context
            var country = await context.Countries
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == CountryId.Of(query.Id), cancellationToken);

            if (country is null)
            {
                throw new ObjectNotFoundException(query.Id);
            }

            //return result
            return new GetCountryByIdResult(country.ToCountryDto());
        }
    }
}
