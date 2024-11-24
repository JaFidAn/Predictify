namespace Prediction.Infrastructure.Data.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.MigrateAsync().GetAwaiter().GetResult();

            await SeedAsync(context);
        }

        private static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedCountryAsync(context);
            await SeedLeagueAsync(context);
            await SeedOutcomeTypeAsync(context);
            await SeedSeasonAsync(context);
        }

        private static async Task SeedCountryAsync(ApplicationDbContext context)
        {
            if (!await context.Countries.AnyAsync())
            {
                await context.Countries.AddRangeAsync(InitialData.Countries);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedLeagueAsync(ApplicationDbContext context)
        {
            if (!await context.Leagues.AnyAsync())
            {
                await context.Leagues.AddRangeAsync(InitialData.Leagues);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedOutcomeTypeAsync(ApplicationDbContext context)
        {
            if (!await context.OutcomeTypes.AnyAsync())
            {
                await context.OutcomeTypes.AddRangeAsync(InitialData.OutcomeTypes);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedSeasonAsync(ApplicationDbContext context)
        {
            if (!await context.Seasons.AnyAsync())
            {
                await context.Seasons.AddRangeAsync(InitialData.Seasons);
                await context.SaveChangesAsync();
            }
        }
    }
}
