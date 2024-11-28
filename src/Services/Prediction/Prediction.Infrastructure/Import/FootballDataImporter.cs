using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Prediction.Application.Data;
using Prediction.Domain.Exceptions;
using Prediction.Application.Extensions;

namespace Prediction.Infrastructure.Import
{
    public class FootballDataImporter : IFootballDataImporter
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<FootballDataImporter> _logger;

        public FootballDataImporter(IApplicationDbContext context, ILogger<FootballDataImporter> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Import initial data (Countries, Leagues, Seasons, Teams, TeamLeagueSeasons)
        public async Task ImportInitialDataAsync(string baseFolderPath, CancellationToken cancellationToken)
        {
            var seasonFolders = Directory.GetDirectories(baseFolderPath);

            foreach (var seasonFolder in seasonFolders)
            {
                var seasonName = Path.GetFileName(seasonFolder); // "2024-25"
                var season = await ResolveSeasonAsync(seasonName, cancellationToken);

                var leagueFiles = Directory.GetFiles(seasonFolder, "*.json");

                foreach (var leagueFile in leagueFiles)
                {
                    await ImportLeagueFileAsync(leagueFile, season.Id, cancellationToken, importMatches: false);
                }
            }
        }

        // Import matches and outcomes
        public async Task ImportMatchesAsync(string baseFolderPath, CancellationToken cancellationToken)
        {
            var seasonFolders = Directory.GetDirectories(baseFolderPath);

            foreach (var seasonFolder in seasonFolders)
            {
                var seasonName = Path.GetFileName(seasonFolder); // "2024-25"
                var season = await ResolveSeasonAsync(seasonName, cancellationToken);

                var leagueFiles = Directory.GetFiles(seasonFolder, "*.json");

                foreach (var leagueFile in leagueFiles)
                {
                    await ImportLeagueFileAsync(leagueFile, season.Id, cancellationToken, importMatches: true);
                }
            }
        }

        private async Task<Season> ResolveSeasonAsync(string seasonFolderName, CancellationToken cancellationToken)
        {
            var seasonName = seasonFolderName.Replace("-", "/"); // Convert "2024-25" to "2024/25"
            var startYear = int.Parse(seasonName.Split('/')[0]);
            var startDate = new DateTime(startYear, 8, 5);
            var endDate = new DateTime(startYear + 1, 8, 3);

            var season = await _context.Seasons.FirstOrDefaultAsync(s => s.Name == seasonName, cancellationToken);
            if (season == null)
            {
                season = Season.Create(SeasonId.Of(Guid.NewGuid()), seasonName, startDate, endDate);
                _context.Seasons.Add(season);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return season;
        }

        private async Task ImportLeagueFileAsync(string leagueFilePath, SeasonId seasonId, CancellationToken cancellationToken, bool importMatches)
        {
            var fileName = Path.GetFileNameWithoutExtension(leagueFilePath); // "Germany-Bundesliga"
            var parts = fileName.Split('-');
            var countryName = parts[0];
            var leagueName = parts[1];

            var country = await ResolveCountryAsync(countryName, cancellationToken);
            var league = await ResolveLeagueAsync(leagueName, country.Id, cancellationToken);

            var json = await File.ReadAllTextAsync(leagueFilePath, cancellationToken);

            try
            {
                // Attempt to deserialize as the old format
                var leagueFileOld = JsonConvert.DeserializeObject<LeagueFileOldFormat>(json);
                if (leagueFileOld?.Rounds != null && leagueFileOld.Rounds.Any())
                {
                    foreach (var round in leagueFileOld.Rounds)
                    {
                        foreach (var matchJson in round.Matches)
                        {
                            if (importMatches)
                            {
                                await ImportMatchOldFormatAsync(matchJson, league.Id, seasonId, cancellationToken);
                            }
                            else
                            {
                                await ImportTeamsFromMatchAsync(matchJson, league.Id, seasonId, cancellationToken);
                            }
                        }
                    }
                    return;
                }
            }
            catch
            {
                _logger.LogDebug("File does not match the old format: {FilePath}", leagueFilePath);
            }

            try
            {
                // Attempt to deserialize as the new format
                var leagueFileNew = JsonConvert.DeserializeObject<LeagueFileNewFormat>(json);
                if (leagueFileNew?.Matches != null && leagueFileNew.Matches.Any())
                {
                    foreach (var matchJson in leagueFileNew.Matches)
                    {
                        if (importMatches)
                        {
                            await ImportMatchNewFormatAsync(matchJson, league.Id, seasonId, cancellationToken);
                        }
                        else
                        {
                            await ImportTeamsFromMatchAsync(matchJson, league.Id, seasonId, cancellationToken);
                        }
                    }
                    return;
                }
            }
            catch
            {
                _logger.LogDebug("File does not match the new format: {FilePath}", leagueFilePath);
            }

            throw new DomainException($"Invalid data in file: {leagueFilePath}");
        }

        private async Task ImportTeamsFromMatchAsync(dynamic matchJson, LeagueId leagueId, SeasonId seasonId, CancellationToken cancellationToken)
        {
            var team1 = await ResolveTeamAsync(matchJson.Team1.ToString(), leagueId, cancellationToken);
            var team2 = await ResolveTeamAsync(matchJson.Team2.ToString(), leagueId, cancellationToken);

            await AddTeamLeagueSeasonAsync(team1.Id, leagueId, seasonId, cancellationToken);
            await AddTeamLeagueSeasonAsync(team2.Id, leagueId, seasonId, cancellationToken);
        }

        private async Task ImportMatchOldFormatAsync(MatchJsonOld matchJson, LeagueId leagueId, SeasonId seasonId, CancellationToken cancellationToken)
        {
            await ImportMatchAsync(matchJson.Date, matchJson.Team1, matchJson.Team2, matchJson.Score.Ft, leagueId, seasonId, cancellationToken);
        }

        private async Task ImportMatchNewFormatAsync(MatchJsonNew matchJson, LeagueId leagueId, SeasonId seasonId, CancellationToken cancellationToken)
        {
            await ImportMatchAsync(matchJson.Date, matchJson.Team1, matchJson.Team2, matchJson.Score.Ft, leagueId, seasonId, cancellationToken);
        }

        private async Task ImportMatchAsync(string date, string team1Name, string team2Name, List<int> score, LeagueId leagueId, SeasonId seasonId, CancellationToken cancellationToken)
        {
            var team1 = await ResolveTeamAsync(team1Name, leagueId, cancellationToken);
            var team2 = await ResolveTeamAsync(team2Name, leagueId, cancellationToken);

            await AddTeamLeagueSeasonAsync(team1.Id, leagueId, seasonId, cancellationToken);
            await AddTeamLeagueSeasonAsync(team2.Id, leagueId, seasonId, cancellationToken);

            if (!_context.Matches.Any(m => m.Team1Id == team1.Id && m.Team2Id == team2.Id && m.Date == DateTime.Parse(date)))
            {
                var match = Match.Create(
                    MatchId.Of(Guid.NewGuid()),
                    team1.Id,
                    team2.Id,
                    DateTime.Parse(date),
                    score.ElementAtOrDefault(0),
                    score.ElementAtOrDefault(1)
                );

                _context.Matches.Add(match);
                await _context.SaveChangesAsync(cancellationToken);

                if (match.IsCompleted)
                {
                    var outcomeTypeIds = await match.DetermineOutcomesAsync(_context, cancellationToken);

                    foreach (var outcomeTypeId in outcomeTypeIds)
                    {
                        var matchOutcome = MatchOutcomeType.Create(MatchOutcomeTypeId.Of(Guid.NewGuid()), match.Id, outcomeTypeId);
                        _context.MatchOutcomeTypes.Add(matchOutcome);
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        private async Task<Country> ResolveCountryAsync(string countryName, CancellationToken cancellationToken)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Name == countryName, cancellationToken);
            if (country == null)
            {
                country = Country.Create(CountryId.Of(Guid.NewGuid()), countryName);
                _context.Countries.Add(country);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return country;
        }

        private async Task<League> ResolveLeagueAsync(string leagueName, CountryId countryId, CancellationToken cancellationToken)
        {
            var league = await _context.Leagues.FirstOrDefaultAsync(l => l.Name == leagueName && l.CountryId == countryId, cancellationToken);
            if (league == null)
            {
                league = League.Create(LeagueId.Of(Guid.NewGuid()), leagueName, countryId);
                _context.Leagues.Add(league);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return league;
        }

        private async Task<Team> ResolveTeamAsync(string teamName, LeagueId leagueId, CancellationToken cancellationToken)
        {
            // Check if the team already exists in the database
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Name == teamName, cancellationToken);

            if (team == null)
            {
                // If the team doesn't exist, create a new one
                _logger.LogInformation("Adding new team: {TeamName} in league: {LeagueId}", teamName, leagueId);

                team = Team.Create(TeamId.Of(Guid.NewGuid()), teamName, leagueId);
                _context.Teams.Add(team);
                await _context.SaveChangesAsync(cancellationToken);
            }
            else if (team.LeagueId != leagueId)
            {
                // If the team exists but the leagueId is different, update the leagueId
                _logger.LogInformation("Updating existing team: {TeamName} with new league: {LeagueId}", teamName, leagueId);

                team.Update(teamName, leagueId);
                _context.Teams.Update(team);
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                // No changes needed
                _logger.LogInformation("Team: {TeamName} already exists with the same league: {LeagueId}", teamName, leagueId);
            }

            return team;
        }

        private async Task AddTeamLeagueSeasonAsync(TeamId teamId, LeagueId leagueId, SeasonId seasonId, CancellationToken cancellationToken)
        {
            if (!_context.TeamLeagueSeasons.Any(tls => tls.TeamId == teamId && tls.LeagueId == leagueId && tls.SeasonId == seasonId))
            {
                var teamLeagueSeason = TeamLeagueSeason.Create(TeamLeagueSeasonId.Of(Guid.NewGuid()), teamId, leagueId, seasonId);
                _context.TeamLeagueSeasons.Add(teamLeagueSeason);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
