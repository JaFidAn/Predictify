namespace Prediction.Application.Features.Countries.Queries.GetCountries
{
    public class GetCountriesHandler(IApplicationDbContext context, ILogger<GetCountriesHandler> logger) : IQueryHandler<GetCountriesQuery, GetCountriesResult>
    {
        public async Task<GetCountriesResult> Handle(GetCountriesQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetCountriesHandler.Handle called with {@Query}", query);

            //get countries with Pagination
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await context.Countries.LongCountAsync(cancellationToken);

            var countries = await context.Countries
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            //return result
            return new GetCountriesResult(
                new PaginatedResult<CountryDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    countries.ToCountryDtoList()));
        }
    }
}
