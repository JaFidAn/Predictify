namespace Prediction.Infrastructure.Data.Configurations
{
    public class SeasonConfiguration : IEntityTypeConfiguration<Season>
    {
        public void Configure(EntityTypeBuilder<Season> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id).HasConversion(
                seasonId => seasonId.Value,
                dbId => SeasonId.Of(dbId));

            builder.Property(s => s.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(s => s.StartDate)
                .IsRequired();

            builder.Property(s => s.EndDate)
                .IsRequired();

            builder.HasIndex(s => s.Name).IsUnique();
        }
    }
}
