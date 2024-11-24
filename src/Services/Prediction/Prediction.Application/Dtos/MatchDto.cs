namespace Prediction.Application.Dtos
{
    public record MatchDto
    {
        public Guid Id { get; init; }
        public Guid Team1Id { get; init; }
        public string? Team1Name { get; init; }
        public Guid Team2Id { get; init; }
        public string? Team2Name { get; init; }
        public DateTime Date { get; init; }
        public int? Team1Goals { get; init; }
        public int? Team2Goals { get; init; }
        public bool? IsCompleted { get; init; }
        public List<OutcomeTypeDto>? OutcomeTypes { get; init; } = new();
    }
}
