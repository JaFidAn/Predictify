namespace Prediction.Domain.ValueObjects
{
    public record TeamHistoryId
    {
        public Guid Value { get; }

        private TeamHistoryId(Guid value) => Value = value;

        public static TeamHistoryId Of(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException("TeamHistoryId cannot be empty.");
            }
            return new TeamHistoryId(value);
        }
    }
}
