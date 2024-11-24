using Prediction.Application.Data;
using System.Reflection;

namespace Prediction.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Country> Countries => Set<Country>();
        public DbSet<League> Leagues => Set<League>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Season> Seasons => Set<Season>();
        public DbSet<TeamLeagueSeason> TeamLeagueSeasons => Set<TeamLeagueSeason>();
        public DbSet<Match> Matches => Set<Match>();
        public DbSet<Forecast> Forecasts => Set<Forecast>();

        public DbSet<OutcomeType> OutcomeTypes => Set<OutcomeType>();
        public DbSet<MatchOutcomeType> MatchOutcomeTypes => Set<MatchOutcomeType>();
        public DbSet<ForecastEvaluation> ForecastEvaluations => Set<ForecastEvaluation>();
        public DbSet<StreakRecord> StreakRecords => Set<StreakRecord>();
        public DbSet<TeamHistory> TeamHistories => Set<TeamHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
