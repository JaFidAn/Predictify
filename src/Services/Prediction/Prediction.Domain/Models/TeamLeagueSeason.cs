namespace Prediction.Domain.Models
{
    public class TeamLeagueSeason : Entity<TeamLeagueSeasonId>
    {
        public TeamId TeamId { get; private set; } = default!;
        public LeagueId LeagueId { get; private set; } = default!;
        public SeasonId SeasonId { get; private set; } = default!;

        private TeamLeagueSeason() { } // For ORM

        public static TeamLeagueSeason Create(TeamLeagueSeasonId id, TeamId teamId, LeagueId leagueId, SeasonId seasonId)
        {
            if (teamId == null || leagueId == null || seasonId == null)
                throw new DomainException("Team, League, and Season must be provided.");

            return new TeamLeagueSeason
            {
                Id = id,
                TeamId = teamId,
                LeagueId = leagueId,
                SeasonId = seasonId
            };
        }

        public void Update(TeamId teamId, LeagueId leagueId, SeasonId seasonId)
        {
            TeamId = teamId;
            LeagueId = leagueId;
            SeasonId = seasonId;
        }
    }
}
