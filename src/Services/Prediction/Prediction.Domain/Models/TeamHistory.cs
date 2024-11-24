namespace Prediction.Domain.Models
{
    public class TeamHistory : Entity<TeamHistoryId>
    {
        public TeamId TeamId { get; private set; } = default!;
        public TeamId OpponentId { get; private set; } = default!;
        public DateTime Date { get; private set; } = default!;
        public int GoalsScored { get; private set; }
        public int GoalsConceded { get; private set; }
        public OutcomeTypeId OutcomeTypeId { get; private set; } = default!;

        private TeamHistory() { } // For ORM

        public static TeamHistory Create(
            TeamHistoryId id,
            TeamId teamId,
            TeamId opponentId,
            DateTime date,
            int goalsScored,
            int goalsConceded,
            OutcomeTypeId outcomeTypeId)
        {
            if (goalsScored < 0 || goalsConceded < 0)
                throw new DomainException("Goals cannot be negative.");

            return new TeamHistory
            {
                Id = id,
                TeamId = teamId,
                OpponentId = opponentId,
                Date = date,
                GoalsScored = goalsScored,
                GoalsConceded = goalsConceded,
                OutcomeTypeId = outcomeTypeId
            };
        }
    }
}
