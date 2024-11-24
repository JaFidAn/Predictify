namespace Prediction.Application.Extensions
{
    public static class LeagueExtensions
    {
        public static IQueryable<LeagueDto> ToLeagueDtoList(this IQueryable<League> leagues, IQueryable<Country> countries)
        {
            return leagues
                .Join(
                    countries,
                    league => league.CountryId,
                    country => country.Id,
                    (league, country) => new LeagueDto
                    {
                        Id = league.Id.Value,
                        Name = league.Name,
                        CountryId = league.CountryId.Value,
                        Country = new CountryDto
                        {
                            Id = country.Id.Value,
                            Name = country.Name
                        }
                    });
        }


        public static LeagueDto ToLeagueDto(this League league, IQueryable<Country> countries)
        {
            var country = countries
                .Where(c => c.Id == league.CountryId)
                .Select(c => new CountryDto
                {
                    Id = c.Id.Value,
                    Name = c.Name
                })
                .FirstOrDefault();

            return new LeagueDto
            {
                Id = league.Id.Value,
                Name = league.Name,
                CountryId = league.CountryId.Value,
                Country = country
            };
        }
    }
}
