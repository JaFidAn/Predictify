namespace Prediction.Application.Dtos
{
    public record TeamHistoryDto
    {
        public Guid Id { get; init; }
        public Guid TeamId { get; init; }
        public string? TeamName { get; init; }
        public Guid OpponentId { get; init; }
        public string? OpponentName { get; init; }
        public DateTime Date { get; init; }
        public int GoalsScored { get; init; }
        public int GoalsConceded { get; init; }
        public Guid OutcomeTypeId { get; init; }
        public string? OutcomeTypeName { get; init; }
    }
}
