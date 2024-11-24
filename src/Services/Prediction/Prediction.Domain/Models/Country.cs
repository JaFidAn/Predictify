namespace Prediction.Domain.Models
{
    public class Country : Entity<CountryId>
    {
        public string Name { get; private set; } = default!;

        private Country() { } // For ORM

        public static Country Create(CountryId id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Country name cannot be null or whitespace.");

            return new Country
            {
                Id = id,
                Name = name
            };
        }

        public void Update(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Country name cannot be null or empty.");

            Name = name;
        }
    }
}
