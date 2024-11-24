namespace Prediction.Infrastructure.Data.Configurations
{
    public class MatchOutcomeTypeConfiguration : IEntityTypeConfiguration<MatchOutcomeType>
    {
        public void Configure(EntityTypeBuilder<MatchOutcomeType> builder)
        {
            builder.HasKey(mo => mo.Id);

            builder.Property(mo => mo.Id).HasConversion(
                matchOutcomeid => matchOutcomeid.Value,
                dbId => MatchOutcomeTypeId.Of(dbId));

            builder.Property(mo => mo.MatchId).HasConversion(
                matchId => matchId.Value,
                dbId => MatchId.Of(dbId));

            builder.Property(mo => mo.OutcomeTypeId).HasConversion(
                outcomeTypeId => outcomeTypeId.Value,
                dbId => OutcomeTypeId.Of(dbId));

            builder.HasOne<Match>()
                .WithMany()
                .HasForeignKey(mo => mo.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<OutcomeType>()
                .WithMany()
                .HasForeignKey(mo => mo.OutcomeTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
