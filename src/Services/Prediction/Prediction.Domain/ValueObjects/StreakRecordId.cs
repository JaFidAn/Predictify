namespace Prediction.Domain.ValueObjects
{
    public record StreakRecordId
    {
        public Guid Value { get; }

        private StreakRecordId(Guid value) => Value = value;

        public static StreakRecordId Of(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException("StreakRecordId cannot be empty.");
            }
            return new StreakRecordId(value);
        }
    }
}
