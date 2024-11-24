using Prediction.Domain.Events;

namespace Prediction.Domain.Models
{
    public class Match : Aggregate<MatchId>
    {
        public DateTime Date { get; private set; } = default!;
        public TeamId Team1Id { get; private set; } = default!;
        public TeamId Team2Id { get; private set; } = default!;
        public int? Team1Goals { get; private set; }
        public int? Team2Goals { get; private set; }
        public bool IsCompleted { get; private set; }

        private readonly List<OutcomeType> _outcomeTypes = new();
        public IReadOnlyCollection<OutcomeType> OutcomeTypes => _outcomeTypes.AsReadOnly();

        private Match() { } // For ORM

        public static Match Create(
            MatchId id,
            TeamId team1Id,
            TeamId team2Id,
            DateTime date,
            int? team1Goals = null,
            int? team2Goals = null)
        {
            if (team1Id == team2Id)
                throw new DomainException("A team cannot play against itself.");

            var match = new Match
            {
                Id = id,
                Team1Id = team1Id,
                Team2Id = team2Id,
                Date = date,
                Team1Goals = team1Goals,
                Team2Goals = team2Goals
            };

            match.UpdateCompletionStatus();

            match.AddDomainEvent(new MatchCreatedEvent(match.Id));

            return match;
        }

        public void Update(TeamId team1Id, TeamId team2Id, DateTime date, int? team1Goals, int? team2Goals)
        {
            Team1Id = team1Id;
            Team2Id = team2Id;
            Date = date;
            Team1Goals = team1Goals;
            Team2Goals = team2Goals;

            UpdateCompletionStatus();

            AddDomainEvent(new MatchUpdatedEvent(Id));

        }

        public void Delete()
        {
            AddDomainEvent(new MatchDeletedEvent(Id));
        }

        private void UpdateCompletionStatus()
        {
            IsCompleted = Team1Goals.HasValue && Team2Goals.HasValue;
        }
    }
}