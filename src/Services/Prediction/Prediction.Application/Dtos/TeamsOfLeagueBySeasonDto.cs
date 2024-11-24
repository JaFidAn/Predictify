namespace Prediction.Application.Dtos
{
    public record TeamsOfLeagueBySeasonDto
    {
        public Guid TeamId { get; init; }
        public string TeamName { get; init; } = default!;
        public Guid LeagueId { get; init; }
        public string LeagueName { get; init; } = default!;
        public Guid SeasonId { get; init; }
        public string SeasonName { get; init; } = default!;
    }
}
