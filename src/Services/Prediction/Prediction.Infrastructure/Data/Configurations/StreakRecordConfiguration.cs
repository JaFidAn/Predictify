namespace Prediction.Infrastructure.Data.Configurations
{
    public class StreakRecordConfiguration : IEntityTypeConfiguration<StreakRecord>
    {
        public void Configure(EntityTypeBuilder<StreakRecord> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id).HasConversion(
                streakRecordId => streakRecordId.Value,
                dbId => StreakRecordId.Of(dbId));

            builder.Property(s => s.TeamId).HasConversion(
                teamId => teamId.Value,
                dbId => TeamId.Of(dbId));

            builder.Property(s => s.OutcomeTypeId).HasConversion(
                outcomeTypeId => outcomeTypeId.Value,
                dbId => OutcomeTypeId.Of(dbId));
        }
    }
}
