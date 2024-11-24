namespace Prediction.Application.Extensions
{
    public static class SeasonExtensions
    {
        public static IEnumerable<SeasonDto> ToSeasonDtoList(this IEnumerable<Season> seasons)
        {
            return seasons.Select(season => new SeasonDto
            {
                Id = season.Id.Value,
                Name = season.Name,
                StartDate = season.StartDate,
                EndDate = season.EndDate
            }).ToList();
        }

        public static SeasonDto ToSeasonDto(this Season season)
        {
            return new SeasonDto
            {
                Id = season.Id.Value,
                Name = season.Name,
                StartDate = season.StartDate,
                EndDate = season.EndDate
            };
        }
    }
}
