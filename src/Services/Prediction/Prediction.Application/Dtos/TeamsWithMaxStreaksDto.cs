namespace Prediction.Application.Dtos
{
    public record TeamsWithMaxStreaksDto
    {
        public string TeamName { get; set; } = default!;
        public string OutcomeTypeName { get; set; } = default!;
        public int CurrentStreak { get; set; }
        public int MaxStreak { get; set; }
    }
}
