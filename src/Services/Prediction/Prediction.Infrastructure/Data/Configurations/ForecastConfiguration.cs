namespace Prediction.Infrastructure.Data.Configurations
{
    public class ForecastConfiguration : IEntityTypeConfiguration<Forecast>
    {
        public void Configure(EntityTypeBuilder<Forecast> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id).HasConversion(
                forecastId => forecastId.Value,
                dbId => ForecastId.Of(dbId));

            builder.Property(f => f.MatchId).HasConversion(
                matchId => matchId.Value,
                dbId => MatchId.Of(dbId));

            builder.Property(f => f.OutcomeTypeId).HasConversion(
                outcomeTypeId => outcomeTypeId.Value,
                dbId => OutcomeTypeId.Of(dbId));

            builder.Property(f => f.Confidence)
                .HasPrecision(3, 2) // Max 3 digits, 2 after the decimal (e.g., 0.95)
                .IsRequired();
        }
    }
}
