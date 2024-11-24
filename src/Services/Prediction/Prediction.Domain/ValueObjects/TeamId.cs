namespace Prediction.Domain.ValueObjects
{
    public record TeamId
    {
        public Guid Value { get; }

        private TeamId(Guid value) => Value = value;

        public static TeamId Of(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException("TeamId cannot be an empty.");
            }
            return new TeamId(value);
        }
    }
}
