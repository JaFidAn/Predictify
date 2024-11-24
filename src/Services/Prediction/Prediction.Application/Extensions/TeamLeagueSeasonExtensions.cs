namespace Prediction.Application.Extensions
{
    public static class TeamLeagueSeasonExtensions
    {
        public static IQueryable<TeamLeagueSeasonDto> ToTeamLeagueSeasonDtoList(this IQueryable<TeamLeagueSeason> teamLeagueSeasons, IQueryable<Team> teams, IQueryable<League> leagues, IQueryable<Season> seasons)
        {
            return teamLeagueSeasons
                .Join(
                    teams,
                    tls => tls.TeamId,
                    team => team.Id,
                    (tls, team) => new { tls, team })
                .Join(
                    leagues,
                    combined => combined.tls.LeagueId,
                    league => league.Id,
                    (combined, league) => new { combined.tls, combined.team, league })
                .Join(
                    seasons,
                    combined => combined.tls.SeasonId,
                    season => season.Id,
                    (combined, season) => new TeamLeagueSeasonDto
                    {
                        Id = combined.tls.Id.Value,
                        TeamId = combined.team.Id.Value,
                        TeamName = combined.team.Name,
                        LeagueId = combined.league.Id.Value,
                        LeagueName = combined.league.Name,
                        SeasonId = season.Id.Value,
                        SeasonName = season.Name
            });
        }


        public static TeamLeagueSeasonDto ToTeamLeagueSeasonDto(this TeamLeagueSeason teamLeagueSeason, IQueryable<Team> teams, IQueryable<League> leagues, IQueryable<Season> seasons)
        {
            var teamWithLeague = teams
                .Join(
                    leagues,
                    team => team.LeagueId,
                    league => league.Id,
                    (team, league) => new { team, league }
                )
                .Where(tl => tl.team.Id == teamLeagueSeason.TeamId)
                .Select(tl => new
                {
                    TeamId = tl.team.Id,
                    TeamName = tl.team.Name,
                    LeagueId = tl.league.Id,
                    LeagueName = tl.league.Name
                })
                .FirstOrDefault();

            var season = seasons
                .Where(s => s.Id == teamLeagueSeason.SeasonId)
                .Select(s => new
                {
                    SeasonId = s.Id,
                    SeasonName = s.Name
                })
                .FirstOrDefault();

            return new TeamLeagueSeasonDto
            {
                Id = teamLeagueSeason.Id.Value,
                TeamId = teamWithLeague?.TeamId.Value ?? Guid.Empty,
                TeamName = teamWithLeague?.TeamName ?? "Unknown",
                LeagueId = teamWithLeague?.LeagueId.Value ?? Guid.Empty,
                LeagueName = teamWithLeague?.LeagueName ?? "Unknown",
                SeasonId = season?.SeasonId.Value ?? Guid.Empty,
                SeasonName = season?.SeasonName ?? "Unknown"
            };
        }
    }
}
