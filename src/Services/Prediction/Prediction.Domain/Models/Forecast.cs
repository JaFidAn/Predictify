namespace Prediction.Domain.Models
{
    public class Forecast : Entity<ForecastId>
    {
        public MatchId MatchId { get; private set; } = default!;
        public OutcomeTypeId OutcomeTypeId { get; private set; } = default!;
        public decimal Confidence { get; private set; } = default!;

        private Forecast() { } // For ORM

        public static Forecast Create(ForecastId id, MatchId matchId, OutcomeTypeId outcomeTypeId, decimal confidence)
        {
            if (confidence < 0 || confidence > 1)
                throw new DomainException("Confidence must be between 0 and 1.");

            return new Forecast
            {
                Id = id,
                MatchId = matchId,
                OutcomeTypeId = outcomeTypeId,
                Confidence = confidence
            };
        }

        public void Update(MatchId matchId, OutcomeTypeId outcomeTypeId, decimal confidence)
        {
            if (confidence < 0 || confidence > 1)
                throw new DomainException("Confidence must be between 0 and 1.");

            MatchId = matchId;
            OutcomeTypeId = outcomeTypeId;
            Confidence = confidence;
        }
    }
}
