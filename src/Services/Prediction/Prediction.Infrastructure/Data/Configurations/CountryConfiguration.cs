namespace Prediction.Infrastructure.Data.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                countryId => countryId.Value,
                dbId => CountryId.Of(dbId));

            builder.Property(c => c.Name).HasMaxLength(50).IsRequired();

            builder.HasIndex(c => c.Name).IsUnique();
        }
    }
}
