namespace Prediction.Domain.ValueObjects
{
    public record SeasonId
    {
        public Guid Value { get; }

        private SeasonId(Guid value) => Value = value;

        public static SeasonId Of(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException("SeasonId cannot be empty.");
            }  
            return new SeasonId(value);
        }

    }
}
