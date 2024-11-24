namespace Prediction.Infrastructure.Data.Configurations
{
    public class LeagueConfiguration : IEntityTypeConfiguration<League>
    {
        public void Configure(EntityTypeBuilder<League> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id).HasConversion(
                leagueId => leagueId.Value,
                dbId => LeagueId.Of(dbId));

            builder.Property(l => l.CountryId).HasConversion(
                countryId => countryId.Value,
                dbId => CountryId.Of(dbId));

            builder.HasOne<Country>()
                .WithMany()
                .HasForeignKey(l => l.CountryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(l => l.Name).HasMaxLength(50).IsRequired();

            builder.HasIndex(l => new { l.Name, l.CountryId }).IsUnique();
        }
    }
}
