namespace Prediction.Domain.ValueObjects
{
    public record MatchId
    {
        public Guid Value { get; }

        private MatchId(Guid value) => Value = value;

        public static MatchId Of(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException("MatchId cannot be empty.");
            }
            return new MatchId(value);
        }
    }
}
