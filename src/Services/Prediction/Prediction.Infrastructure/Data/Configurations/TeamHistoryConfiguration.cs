namespace Prediction.Infrastructure.Data.Configurations
{
    public class TeamHistoryConfiguration : IEntityTypeConfiguration<TeamHistory>
    {
        public void Configure(EntityTypeBuilder<TeamHistory> builder)
        {
            builder.HasKey(th => th.Id);

            builder.Property(th => th.Id).HasConversion(
                teamHistoryId => teamHistoryId.Value,
                dbId => TeamHistoryId.Of(dbId));

            builder.Property(th => th.TeamId).HasConversion(
                teamId => teamId.Value,
                dbId => TeamId.Of(dbId));

            builder.Property(th => th.OpponentId).HasConversion(
                opponentId => opponentId.Value,
                dbId => TeamId.Of(dbId));

            builder.Property(th => th.OutcomeTypeId).HasConversion(
                outcomeTypeId => outcomeTypeId.Value,
                dbId => OutcomeTypeId.Of(dbId));

            builder.Property(th => th.Date)
                .IsRequired();

            builder.Property(th => th.GoalsScored).IsRequired();

            builder.Property(th => th.GoalsConceded).IsRequired();
        }
    }
}
