namespace Prediction.Infrastructure.Data.Configurations
{
    public class TeamLeagueSeasonConfiguration : IEntityTypeConfiguration<TeamLeagueSeason>
    {
        public void Configure(EntityTypeBuilder<TeamLeagueSeason> builder)
        {
            builder.HasKey(tls => tls.Id);

            builder.Property(tls => tls.Id).HasConversion(
                id => id.Value,
                dbId => TeamLeagueSeasonId.Of(dbId));

            builder.Property(tls => tls.TeamId).HasConversion(
                teamId => teamId.Value,
                dbId => TeamId.Of(dbId));

            builder.Property(tls => tls.LeagueId).HasConversion(
                leagueId => leagueId.Value,
                dbId => LeagueId.Of(dbId));

            builder.Property(tls => tls.SeasonId).HasConversion(
                seasonId => seasonId.Value,
                dbId => SeasonId.Of(dbId));

            builder.HasOne<Team>()
                .WithMany()
                .HasForeignKey(tls => tls.TeamId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<League>()
                .WithMany()
                .HasForeignKey(tls => tls.LeagueId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<Season>()
                .WithMany()
                .HasForeignKey(tls => tls.SeasonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(tls => new { tls.TeamId, tls.LeagueId, tls.SeasonId }).IsUnique();
        }
    }
}
