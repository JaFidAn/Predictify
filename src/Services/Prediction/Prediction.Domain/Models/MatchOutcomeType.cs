namespace Prediction.Domain.Models
{
    public class MatchOutcomeType : Entity<MatchOutcomeTypeId> 
    {
        public MatchId MatchId { get; private set; } = default!;
        public OutcomeTypeId OutcomeTypeId { get; private set; } = default!;

        private MatchOutcomeType() { } // For ORM

        public static MatchOutcomeType Create(MatchOutcomeTypeId id, MatchId matchId, OutcomeTypeId outcomeTypeId)
        {
            return new MatchOutcomeType
            {
                Id = id,
                MatchId = matchId,
                OutcomeTypeId = outcomeTypeId
            };
        }

        public void Update(MatchId matchId, OutcomeTypeId outcomeTypeId)
        {
            MatchId = matchId;
            OutcomeTypeId = outcomeTypeId;
        }

    }
}
