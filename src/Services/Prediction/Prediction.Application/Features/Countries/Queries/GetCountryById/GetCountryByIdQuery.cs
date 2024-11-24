namespace Prediction.Application.Features.Countries.Queries.GetCountryById
{
    public record GetCountryByIdQuery(Guid Id) : IQuery<GetCountryByIdResult>;
    public record GetCountryByIdResult(CountryDto Country);
}
