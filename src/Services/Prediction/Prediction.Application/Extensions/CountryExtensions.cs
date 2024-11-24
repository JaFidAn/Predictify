namespace Prediction.Application.Extensions
{
    public static class CountryExtensions
    {
        public static IEnumerable<CountryDto> ToCountryDtoList(this IEnumerable<Country> countries)
        {
            return countries.Select(country => new CountryDto
            {
                Id = country.Id.Value,
                Name = country.Name,
            }).ToList();
        }

        public static CountryDto ToCountryDto(this Country country)
        {
            return new CountryDto
            {
                Id = country.Id.Value,
                Name = country.Name,
            };
        }
    }
}
