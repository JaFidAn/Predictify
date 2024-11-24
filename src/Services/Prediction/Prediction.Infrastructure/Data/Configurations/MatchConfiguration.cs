namespace Prediction.Infrastructure.Data.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).HasConversion(
                matchId => matchId.Value,
                dbId => MatchId.Of(dbId));

            builder.Property(m => m.Team1Id).HasConversion(
                teamId => teamId.Value,
                dbId => TeamId.Of(dbId));

            builder.Property(m => m.Team2Id).HasConversion(
                teamId => teamId.Value,
                dbId => TeamId.Of(dbId));

            builder.HasOne<Team>()
                .WithMany()
                .HasForeignKey(m => m.Team1Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<Team>()
                .WithMany()
                .HasForeignKey(m => m.Team2Id)
                .OnDelete(DeleteBehavior.NoAction);
                        
            builder.Property(m => m.Team1Goals).IsRequired(false);

            builder.Property(m => m.Team2Goals).IsRequired(false);
                        
            builder.HasIndex(m => new { m.Team1Id, m.Team2Id, m.Date }).IsUnique();

            // Ignore Domain-Only Properties
            builder.Ignore(m => m.OutcomeTypes);
        }
    }
}
