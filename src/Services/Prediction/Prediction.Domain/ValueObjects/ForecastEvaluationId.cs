namespace Prediction.Domain.ValueObjects
{
    public record ForecastEvaluationId
    {
        public Guid Value { get; }

        private ForecastEvaluationId(Guid value) => Value = value;

        public static ForecastEvaluationId Of(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException("ForecastEvaluationId cannot be empty.");
            }
            return new ForecastEvaluationId(value);
        }
    }
}
