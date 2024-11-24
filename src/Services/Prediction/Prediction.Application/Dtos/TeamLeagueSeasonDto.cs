namespace Prediction.Application.Dtos
{
    public record TeamLeagueSeasonDto
    {
        public Guid Id { get; init; }
        public Guid TeamId { get; init; }
        public string? TeamName { get; init; }
        public Guid LeagueId { get; init; }
        public string? LeagueName { get; init; }
        public Guid SeasonId { get; init; }
        public string? SeasonName { get; init; }
    }
}
