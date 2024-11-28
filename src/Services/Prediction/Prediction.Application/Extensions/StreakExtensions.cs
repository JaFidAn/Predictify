using Prediction.Domain.Exceptions;

namespace Prediction.Application.Extensions
{
    public static class StreakExtensions
    {
        public static async Task UpdateStreaksAsync(this TeamId teamId, IApplicationDbContext context, CancellationToken cancellationToken)
        {
            var matches = await context.Matches
                .Where(m => m.Team1Id == teamId || m.Team2Id == teamId)
                .OrderBy(m => m.Date)
                .ToListAsync(cancellationToken);

            if (!matches.Any())
                return;

            var outcomeTypes = await context.OutcomeTypes.ToListAsync(cancellationToken);
            var outcomeTypeMap = outcomeTypes.ToDictionary(
                ot => ot.Id,
                ot => OutcomeTypeId.Of(ot.Id.Value)
            );

            var existingStreakRecords = await context.StreakRecords
                .Where(sr => sr.TeamId == teamId)
                .ToListAsync(cancellationToken);

            var streakRecordMap = existingStreakRecords.ToDictionary(
                sr => sr.OutcomeTypeId,
                sr => sr
            );

            var streaks = outcomeTypeMap.Values.ToDictionary(
                id => id,
                id => (CurrentStreak: 0, MaxStreak: 0)
            );

            foreach (var match in matches)
            {
                var applicableOutcomes = await match.GetApplicableOutcomesAsync(context, teamId, cancellationToken);

                foreach (var outcomeTypeId in outcomeTypeMap.Values)
                {
                    var outcomeOccurred = applicableOutcomes.Contains(outcomeTypeId);

                    if (!outcomeOccurred)
                    {
                        streaks[outcomeTypeId] = (
                            CurrentStreak: streaks[outcomeTypeId].CurrentStreak + 1,
                            MaxStreak: Math.Max(streaks[outcomeTypeId].MaxStreak, streaks[outcomeTypeId].CurrentStreak + 1)
                        );
                    }
                    else
                    {
                        streaks[outcomeTypeId] = (
                            CurrentStreak: 0,
                            MaxStreak: streaks[outcomeTypeId].MaxStreak
                        );
                    }
                }
            }

            foreach (var (outcomeTypeId, (currentStreak, maxStreak)) in streaks)
            {
                if (streakRecordMap.TryGetValue(outcomeTypeId, out var existingRecord))
                {
                    existingRecord.UpdateStreak(currentStreak, maxStreak);
                }
                else
                {
                    var newStreakRecord = StreakRecord.Create(
                        StreakRecordId.Of(Guid.NewGuid()),
                        teamId,
                        outcomeTypeId,
                        currentStreak,
                        maxStreak
                    );
                    context.StreakRecords.Add(newStreakRecord);
                }
            }

            await context.SaveChangesAsync(cancellationToken);
        }

        public static async Task<IEnumerable<OutcomeTypeId>> GetApplicableOutcomesAsync(this Match match, IApplicationDbContext context, TeamId teamId, CancellationToken cancellationToken)
        {
            if (!match.Team1Goals.HasValue || !match.Team2Goals.HasValue)
            {
                return Enumerable.Empty<OutcomeTypeId>();
            }

            // Fetch the outcome type map using the centralized method
            var outcomeTypeMap = await context.GetOutcomeTypeMapAsync(cancellationToken);

            var applicableOutcomes = new List<OutcomeTypeId>();

            // Determine if the given TeamId matches one of the teams in the match
            bool isTeam1 = match.Team1Id == teamId;
            bool isTeam2 = match.Team2Id == teamId;

            if (!isTeam1 && !isTeam2)
            {
                throw new DomainException("The given TeamId does not match Team1Id or Team2Id in the match.");
            }

            // Determine Win, Loss, or Draw
            if (match.Team1Goals.Value > match.Team2Goals.Value)
            {
                applicableOutcomes.Add(isTeam1 ? outcomeTypeMap["Win"] : outcomeTypeMap["Loss"]);
            }
            else if (match.Team1Goals.Value < match.Team2Goals.Value)
            {
                applicableOutcomes.Add(isTeam1 ? outcomeTypeMap["Loss"] : outcomeTypeMap["Win"]);
            }
            else
            {
                applicableOutcomes.Add(outcomeTypeMap["Draw"]);
            }

            // Determine Over_2_5 or Under_2_5
            var totalGoals = match.Team1Goals.Value + match.Team2Goals.Value;
            applicableOutcomes.Add(totalGoals > 2.5 ? outcomeTypeMap["Over_2_5"] : outcomeTypeMap["Under_2_5"]);

            return applicableOutcomes;
        }
    }
}