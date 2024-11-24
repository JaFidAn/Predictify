namespace Prediction.Domain.Models
{
    public class ForecastEvaluation : Entity<ForecastEvaluationId>
    {
        public ForecastId ForecastId { get; private set; } = default!;
        public OutcomeTypeId ActualOutcomeId { get; private set; } = default!;
        public decimal ConfidenceScore { get; private set; }
        public bool WasCorrect { get; private set; }

        private ForecastEvaluation() { } // For ORM

        public static ForecastEvaluation Create(
            ForecastEvaluationId id,
            ForecastId forecastId,
            OutcomeTypeId actualOutcomeId,
            decimal confidenceScore,
            bool wasCorrect)
        {
            if (confidenceScore < 0 || confidenceScore > 1)
                throw new DomainException("Confidence score must be between 0 and 1.");

            return new ForecastEvaluation
            {
                Id = id,
                ForecastId = forecastId,
                ActualOutcomeId = actualOutcomeId,
                ConfidenceScore = confidenceScore,
                WasCorrect = wasCorrect
            };
        }
    }
}
