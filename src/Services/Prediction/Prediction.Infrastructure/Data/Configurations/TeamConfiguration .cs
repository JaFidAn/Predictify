namespace Prediction.Infrastructure.Data.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasConversion(
                teamId => teamId.Value,
                dbId => TeamId.Of(dbId));

            builder.Property(t => t.LeagueId).HasConversion(
                leagueId => leagueId.Value,
                dbId => LeagueId.Of(dbId));

            builder.HasOne<League>()
                .WithMany()
                .HasForeignKey(t => t.LeagueId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(t => t.Name).HasMaxLength(50).IsRequired();            

            builder.HasIndex(t => new { t.Name, t.LeagueId }).IsUnique();
        }
    }
}
