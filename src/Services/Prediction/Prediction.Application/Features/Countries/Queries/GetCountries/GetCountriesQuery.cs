namespace Prediction.Application.Features.Countries.Queries.GetCountries
{
    public record GetCountriesQuery(PaginationRequest PaginationRequest) : IQuery<GetCountriesResult>;
    public record GetCountriesResult(PaginatedResult<CountryDto> Countries);
}
