using Microsoft.EntityFrameworkCore;

namespace Prediction.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Country> Countries { get; }
        DbSet<League> Leagues {  get; }
        DbSet<Team> Teams {  get; }
        DbSet<Season> Seasons { get; }
        DbSet<TeamLeagueSeason> TeamLeagueSeasons { get; }
        DbSet<Match> Matches { get; }
        DbSet<Forecast> Forecasts { get; }

        DbSet<OutcomeType> OutcomeTypes { get; }
        DbSet<MatchOutcomeType> MatchOutcomeTypes {  get; } 
        DbSet<ForecastEvaluation> ForecastEvaluations { get; }
        DbSet<StreakRecord> StreakRecords {  get; }
        DbSet<TeamHistory> TeamHistories { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
