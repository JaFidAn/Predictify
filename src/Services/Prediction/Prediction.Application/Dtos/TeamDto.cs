namespace Prediction.Application.Dtos
{
    public record TeamDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public Guid LeagueId { get; init; }
        public LeagueDto? League { get; init; }
    }
}
