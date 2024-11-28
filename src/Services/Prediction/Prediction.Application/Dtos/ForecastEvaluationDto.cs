namespace Prediction.Application.Dtos
{
    public record ForecastEvaluationDto
    {
        public Guid Id { get; init; }
        public DateTime Date { get; init; }
        public string? Team1Name { get; init; }
        public string? Team2Name { get; init; }
        public DateTime MatchDate { get; init; }
        public Guid ForecastOutcome { get; init; }
        public string? ForecastOutcomeName { get; init; }
        public List<OutcomeTypeDto>? ActualOutcomes { get; init; } 
        public decimal Confidence { get; init; }
        public bool WasCorrect { get; init; }
    }
}
