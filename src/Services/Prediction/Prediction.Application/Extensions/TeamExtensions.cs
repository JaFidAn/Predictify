namespace Prediction.Application.Extensions
{
    public static class TeamExtensions
    {
            public static async Task<List<TeamDto>> ToTeamDtoListAsync(this IQueryable<Team> teams,IQueryable<League> leagues, IQueryable<Country> countries, CancellationToken cancellationToken = default)
        {
            return await teams
                .Join(leagues,
                    team => team.LeagueId,
                    league => league.Id,
                    (team, league) => new { team, league })
                .Join(countries,
                    teamLeague => teamLeague.league.CountryId,
                    country => country.Id,
                    (teamLeague, country) => new TeamDto
                    {
                        Id = teamLeague.team.Id.Value,
                        Name = teamLeague.team.Name,
                        LeagueId = teamLeague.team.LeagueId.Value,
                        League = new LeagueDto
                        {
                            Id = teamLeague.league.Id.Value,
                            Name = teamLeague.league.Name,
                            CountryId = teamLeague.league.CountryId.Value,
                            Country = new CountryDto
                            {
                                Id = country.Id.Value,
                                Name = country.Name,
                            }
                        }
                    })
                .ToListAsync(cancellationToken);
        }


        public static TeamDto ToTeamDto(this Team team, IQueryable<League> leagues, IQueryable<Country> countries)
        {
            var leagueWithCountry = leagues
                .Join(
                    countries,
                    league => league.CountryId,
                    country => country.Id,
                    (league, country) => new { league, country }
                )
                .Where(lc => lc.league.Id == team.LeagueId)
                .Select(lc => new LeagueDto
                {
                    Id = lc.league.Id.Value,
                    Name = lc.league.Name,
                    CountryId = lc.league.CountryId.Value,
                    Country = new CountryDto
                    {
                        Id = lc.country.Id.Value,
                        Name = lc.country.Name
                    }
                })
                .FirstOrDefault();

            return new TeamDto
            {
                Id = team.Id.Value,
                Name = team.Name,
                LeagueId = team.LeagueId.Value,
                League = leagueWithCountry
            };
        }

        public static IQueryable<TeamsOfLeagueBySeasonDto> TeamsOfLeagueBySeasonToDto(this IQueryable<TeamLeagueSeason> teamLeagueSeasons, IQueryable<Team> teams, IQueryable<League> leagues, IQueryable<Season> seasons, Guid leagueId,Guid seasonId)
        {
            return teamLeagueSeasons
                .Where(tls => tls.LeagueId == LeagueId.Of(leagueId) && tls.SeasonId == SeasonId.Of(seasonId))
                .Join(
                    teams,
                    tls => tls.TeamId,
                    team => team.Id,
                    (tls, team) => new { tls, team })
                .Join(
                    leagues,
                    combined => combined.tls.LeagueId,
                    league => league.Id,
                    (combined, league) => new { combined, league })
                .Join(
                    seasons,
                    combined => combined.combined.tls.SeasonId,
                    season => season.Id,
                    (combined, season) => new TeamsOfLeagueBySeasonDto
                    {
                        TeamId = combined.combined.team.Id.Value,
                        TeamName = combined.combined.team.Name,
                        LeagueId = combined.league.Id.Value,
                        LeagueName = combined.league.Name,
                        SeasonId = season.Id.Value,
                        SeasonName = season.Name
                    });
        }

        public static async Task<Dictionary<OutcomeTypeId, (int CurrentStreak, int MaxStreak)>> GetTeamStreaksAsync(this TeamId teamId, IApplicationDbContext context, CancellationToken cancellationToken)
        {
            // Fetch streak records for the team
            var streakRecords = await context.StreakRecords
                .Where(sr => sr.TeamId == teamId)
                .ToListAsync(cancellationToken);

            if (!streakRecords.Any())
            {
                // Return an empty dictionary for no streaks
                return new Dictionary<OutcomeTypeId, (int CurrentStreak, int MaxStreak)>();
            }

            // Convert streak records into a dictionary
            return streakRecords.ToDictionary(
                sr => sr.OutcomeTypeId,
                sr => (sr.CurrentStreak, sr.MaxStreak > 0 ? sr.MaxStreak : 1) // Ensure MaxStreak is at least 1
            );
        }
    }
}
