namespace Prediction.Domain.Models
{
    public class Season : Entity<SeasonId>
    {
        public string Name { get; private set; } = default!; // e.g., "2024/2025"
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        private Season() { } // For ORM

        public static Season Create(SeasonId id, string name, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Season name cannot be null or whitespace.");

            if (startDate >= endDate)
                throw new DomainException("Season start date must be earlier than the end date.");

            return new Season
            {
                Id = id,
                Name = name,
                StartDate = startDate,
                EndDate = endDate
            };
        }

        public void Update(string name, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Season name cannot be null or whitespace.");

            if (startDate >= endDate)
                throw new DomainException("Season start date must be earlier than the end date.");

            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
