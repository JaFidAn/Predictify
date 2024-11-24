namespace Prediction.Application.Dtos
{
    public record LeagueDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public Guid CountryId { get; init; }
        public CountryDto? Country { get; init; }
    }
}
