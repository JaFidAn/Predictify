namespace Prediction.Application.Features.Streaks.Queries.GetStreaksForTeam
{
    public class GetStreaksForTeamHandler(IApplicationDbContext context, ILogger<GetStreaksForTeamHandler> logger) : IQueryHandler<GetStreaksForTeamQuery, GetStreaksForTeamResult>
    {
        public async Task<GetStreaksForTeamResult> Handle(GetStreaksForTeamQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetStreaksForTeamHandler.Handle called with {@Query}", query);

            var teamId = TeamId.Of(query.TeamId);

            // Fetch the team
            var team = await context.Teams
                .Where(t => t.Id == teamId)
                .Select(t => new { t.Id, t.Name })
                .FirstOrDefaultAsync(cancellationToken);

            if (team is null)
            {
                throw new ObjectNotFoundException(query.TeamId);
            }

            // Fetch streak records
            var streakRecords = await context.StreakRecords
                .Where(sr => sr.TeamId == teamId)
                .ToListAsync(cancellationToken);

            // Fetch outcome types
            var outcomeTypes = await context.OutcomeTypes.ToListAsync(cancellationToken);

            // Map outcome type names to streak data
            var streaks = outcomeTypes
                .ToDictionary(
                    ot => ot.Name,
                    ot =>
                    {
                        var streakRecord = streakRecords.FirstOrDefault(sr => sr.OutcomeTypeId == ot.Id);

                        return new StreakDto
                        {
                            CurrentStreak = streakRecord?.CurrentStreak ?? 0,
                            MaximumStreak = streakRecord?.MaxStreak ?? 0
                        };
                    });

            // Return the result
            return new GetStreaksForTeamResult(team.Id.Value, team.Name, streaks);
        }
    }
}
