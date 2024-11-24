namespace Prediction.Domain.Models
{
    public class OutcomeType : Entity<OutcomeTypeId>
    {
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }

        private OutcomeType() { } // For ORM

        public static OutcomeType Create(OutcomeTypeId id, string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("OutcomeType name cannot be null or whitespace.");

            return new OutcomeType
            {
                Id = id,
                Name = name,
                Description = description
            };
        }

        public void Update(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("OutcomeType name cannot be null or empty.");

            Name = name;
            Description = description;
        }
    }
}
