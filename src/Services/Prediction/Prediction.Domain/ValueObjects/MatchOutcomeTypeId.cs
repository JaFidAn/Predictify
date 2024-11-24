namespace Prediction.Domain.ValueObjects
{
    public record MatchOutcomeTypeId
    {
        public Guid Value { get; }

        private MatchOutcomeTypeId(Guid value) => Value = value;

        public static MatchOutcomeTypeId Of(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException("MatchOutcomeTypeId cannot be empty.");
            }
            return new MatchOutcomeTypeId(value);
        }
    }
}
