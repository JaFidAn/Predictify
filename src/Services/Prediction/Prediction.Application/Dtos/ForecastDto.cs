namespace Prediction.Application.Dtos
{
    public record ForecastDto
    {
        public Guid ForecastId { get; init; }
        public Guid MatchId { get; init; }
        public Guid OutcomeTypeId { get; init; }
        public string? OutcomeTypeName { get; init; }
        public decimal Confidence { get; init; }
        public Guid Team1Id { get; init; }
        public string? Team1Name { get; init; }
        public Guid Team2Id { get; init; }
        public string? Team2Name { get; init; }
        public DateTime MatchDate { get; init; }
    }
}
