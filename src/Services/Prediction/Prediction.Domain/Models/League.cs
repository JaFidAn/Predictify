namespace Prediction.Domain.Models
{
    public class League : Entity<LeagueId>
    {
        public string Name { get; private set; } = default!;
        public CountryId CountryId { get; private set; } = default!;

        private League() { } // For ORM

        public static League Create(LeagueId id, string name, CountryId countryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("League name cannot be null or whitespace.");

            return new League
            {
                Id = id,
                Name = name,
                CountryId = countryId
            };
        }

        public void Update(string name, CountryId countryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Team name cannot be null or empty.");

            Name = name;
            CountryId = countryId;
        }
    }
}
