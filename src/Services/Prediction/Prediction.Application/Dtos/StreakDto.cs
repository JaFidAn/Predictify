namespace Prediction.Application.Dtos
{
    public record StreakDto
    {
        public int CurrentStreak { get; init; }
        public int MaximumStreak { get; init; }
    }
}
