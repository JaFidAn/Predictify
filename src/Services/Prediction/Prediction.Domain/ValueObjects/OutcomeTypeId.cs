namespace Prediction.Domain.ValueObjects
{
    public record OutcomeTypeId
    {
        public Guid Value { get; }

        private OutcomeTypeId(Guid value) => Value = value;

        public static OutcomeTypeId Of(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException("OutcomeTypeId cannot be empty.");
            }
            return new OutcomeTypeId(value);
        }
    }
}
