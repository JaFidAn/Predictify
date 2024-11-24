namespace Prediction.Domain.Models
{
    public class StreakRecord : Aggregate<StreakRecordId>
    {
        public TeamId TeamId { get; private set; } = default!;
        public OutcomeTypeId OutcomeTypeId { get; private set; } = default!;
        public int CurrentStreak { get; private set; } = 0; // Tracks the ongoing streak of absences for the outcome type
        public int MaxStreak { get; private set; } = 0;     // Tracks the maximum streak ever achieved

        private StreakRecord() { } // For ORM

        // Factory method for creating a new streak record
        public static StreakRecord Create(
            StreakRecordId id,
            TeamId teamId,
            OutcomeTypeId outcomeTypeId,
            int currentStreak = 0,
            int maxStreak = 0)
        {
            if (currentStreak < 0 || maxStreak < 0)
                throw new DomainException("Streak values cannot be negative.");

            return new StreakRecord
            {
                Id = id,
                TeamId = teamId,
                OutcomeTypeId = outcomeTypeId,
                CurrentStreak = currentStreak,
                MaxStreak = maxStreak
            };
        }

        // Update streaks based on calculated values
        public void UpdateStreak(int currentStreak, int maxStreak)
        {
            if (currentStreak < 0 || maxStreak < 0)
                throw new DomainException("Streak values cannot be negative.");

            CurrentStreak = currentStreak;

            // Ensure MaxStreak reflects the highest streak achieved
            MaxStreak = Math.Max(MaxStreak, maxStreak);
        }

        // Validate streak values
        public void EnsureValidStreaks()
        {
            if (CurrentStreak < 0 || MaxStreak < 0)
                throw new DomainException("Streak values cannot be negative.");

            if (CurrentStreak > MaxStreak)
                throw new DomainException("CurrentStreak cannot exceed MaxStreak.");
        }
    }
}
