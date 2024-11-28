namespace Prediction.Infrastructure.Data.Configurations
{
    public class ForecastEvaluationConfiguration : IEntityTypeConfiguration<ForecastEvaluation>
    {
        public void Configure(EntityTypeBuilder<ForecastEvaluation> builder)
        {
            builder.HasKey(fe => fe.Id);

            builder.Property(fe => fe.Id).HasConversion(
                id => id.Value,
                dbId => ForecastEvaluationId.Of(dbId));

            builder.Property(fe => fe.ForecastId).HasConversion(
                forecastId => forecastId.Value,
                dbId => ForecastId.Of(dbId));

            builder.Property(fe => fe.MatchId).HasConversion(
            matchId => matchId.Value,
            dbId => MatchId.Of(dbId));

            builder.Property(fe => fe.ForecastOutcomeId).HasConversion(
            outcomeId => outcomeId.Value,
            dbId => OutcomeTypeId.Of(dbId));

            builder.Property(fe => fe.ConfidenceScore)
                .HasPrecision(3, 2) // Max 3 digits, 2 after the decimal (e.g., 0.98)
                .IsRequired();

            builder.Property(fe => fe.WasCorrect).IsRequired();
        }
    }
}
