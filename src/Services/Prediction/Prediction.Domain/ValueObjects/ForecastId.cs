namespace Prediction.Domain.ValueObjects
{
    public record ForecastId
    {
        public Guid Value { get; }

        private ForecastId(Guid value) => Value = value;

        public static ForecastId Of(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException("ForecastId cannot be empty.");
            }
            return new ForecastId(value);
        }
    }
}
