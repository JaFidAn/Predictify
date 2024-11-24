namespace Prediction.Domain.ValueObjects
{
    public record CountryId
    {
        public Guid Value { get; }

        private CountryId(Guid value) => Value = value;

        public static CountryId Of(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException("CountryId cannot be empty.");
            }
            return new CountryId(value);
        }
    }
}
