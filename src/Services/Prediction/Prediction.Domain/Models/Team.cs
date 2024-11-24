namespace Prediction.Domain.Models
{
    public class Team : Entity<TeamId>
    {
        public string Name { get; private set; } = default!;
        public LeagueId LeagueId { get; private set; } = default!;

        private Team() { } // For ORM

        public static Team Create(TeamId id, string name, LeagueId leagueId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Team name cannot be null or whitespace.");

            return new Team
            {
                Id = id,
                Name = name,
                LeagueId = leagueId
            };
        }

        public void Update(string name, LeagueId leagueId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Team name cannot be null or empty.");

            Name = name;
            LeagueId = leagueId;
        }
    }
}
