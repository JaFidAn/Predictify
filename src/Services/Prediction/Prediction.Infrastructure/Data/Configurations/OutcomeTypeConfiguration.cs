namespace Prediction.Infrastructure.Data.Configurations
{
    public class OutcomeTypeConfiguration : IEntityTypeConfiguration<OutcomeType>
    {
        public void Configure(EntityTypeBuilder<OutcomeType> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id).HasConversion(
                outcomeTypeId => outcomeTypeId.Value,
                dbId => OutcomeTypeId.Of(dbId));

            builder.Property(o => o.Name)
                .HasMaxLength(20)
                .IsRequired();

           

            builder.HasIndex(o => o.Name).IsUnique();
        }
    }
}
