using Prediction.Domain.Exceptions;


namespace Prediction.Application.Extensions
{
    public static class MatchExtensions
    {
        public static async Task ValidateTeamsExistAsync(this MatchDto matchDto, IApplicationDbContext context, CancellationToken cancellationToken)
        {
            var team1Exists = await context.Teams.AnyAsync(t => t.Id == TeamId.Of(matchDto.Team1Id), cancellationToken);
            var team2Exists = await context.Teams.AnyAsync(t => t.Id == TeamId.Of(matchDto.Team2Id), cancellationToken);

            if (!team1Exists)
                throw new ObjectNotFoundException(matchDto.Team1Id, nameof(Team));

            if (!team2Exists)
                throw new ObjectNotFoundException(matchDto.Team2Id, nameof(Team));
        }

        public static async Task CheckForDuplicateMatchAsync(this MatchDto matchDto, IApplicationDbContext context, MatchId? existingMatchId = null,CancellationToken cancellationToken = default)
        {
            // Fetch matches where either of the teams is playing on the same date
            var isDuplicate = await context.Matches
                .Where(m =>
                    m.Date.Date == matchDto.Date.Date && // Matches on the same date
                    (m.Team1Id == TeamId.Of(matchDto.Team1Id) || // Either Team1
                     m.Team2Id == TeamId.Of(matchDto.Team1Id) || // Or Team2 plays
                     m.Team1Id == TeamId.Of(matchDto.Team2Id) || // Or the opposing Team1
                     m.Team2Id == TeamId.Of(matchDto.Team2Id)) && // Or the opposing Team2
                    (existingMatchId == null || m.Id != existingMatchId)) // Exclude the current match if updating
                .AnyAsync(cancellationToken);

            if (isDuplicate)
            {
                throw new DomainException($"One of the teams (Team1Id {matchDto.Team1Id} or Team2Id {matchDto.Team2Id}) is already scheduled to play on {matchDto.Date.Date}.");
            }
        }

        public static async Task<IEnumerable<OutcomeTypeId>> DetermineOutcomesAsync(this Match match, IApplicationDbContext context, CancellationToken cancellationToken)
        {
            if (!match.IsCompleted)
            {
                throw new DomainException("Cannot determine outcomes for an incomplete match.");
            }

            // Fetch the outcome type map using the centralized method
            var outcomeTypeMap = await context.GetOutcomeTypeMapAsync(cancellationToken);

            var outcomes = new List<OutcomeTypeId>();

            // Determine Win, Loss, or Draw
            if (match.Team1Goals > match.Team2Goals)
            {
                outcomes.Add(outcomeTypeMap["Win"]);
            }
            else if (match.Team1Goals < match.Team2Goals)
            {
                outcomes.Add(outcomeTypeMap["Loss"]);
            }
            else
            {
                outcomes.Add(outcomeTypeMap["Draw"]);
            }

            // Determine Over_2_5 or Under_2_5
            int totalGoals = match.Team1Goals.Value + match.Team2Goals.Value;
            outcomes.Add(totalGoals > 2.5 ? outcomeTypeMap["Over_2_5"] : outcomeTypeMap["Under_2_5"]);

            return outcomes;
        }


        public static async Task AddMatchOutcomeTypesAsync(this Match match, IEnumerable<OutcomeTypeId> outcomeTypeIds, IApplicationDbContext context, CancellationToken cancellationToken)
        {
            foreach (var outcomeTypeId in outcomeTypeIds)
            {
                var matchOutcomeType = MatchOutcomeType.Create(
                    MatchOutcomeTypeId.Of(Guid.NewGuid()),
                    match.Id,
                    outcomeTypeId
                );

                await context.MatchOutcomeTypes.AddAsync(matchOutcomeType, cancellationToken);
            }
        }

        public static async Task UpdateMatchOutcomesAsync(this Match match, IApplicationDbContext context, CancellationToken cancellationToken)
        {
            if (!match.IsCompleted)
            {
                throw new InvalidOperationException("Cannot update outcomes for an incomplete match.");
            }

            // Fetch existing outcomes for this match
            var existingOutcomes = await context.MatchOutcomeTypes
                .Where(mot => mot.MatchId == match.Id)
                .ToListAsync(cancellationToken);

            // Determine the new outcomes for the updated match
            var newOutcomeIds = await match.DetermineOutcomesAsync(context, cancellationToken);

            // Find outcomes to delete (in existing but not in new)
            var outcomesToDelete = existingOutcomes
                .Where(existing => !newOutcomeIds.Contains(existing.OutcomeTypeId))
                .ToList();

            // Find outcomes to add (in new but not in existing)
            var newOutcomesToAdd = newOutcomeIds
                .Where(newOutcomeId => !existingOutcomes.Any(existing => existing.OutcomeTypeId == newOutcomeId))
                .ToList();

            // Delete outdated outcomes
            context.MatchOutcomeTypes.RemoveRange(outcomesToDelete);

            // Add new outcomes
            foreach (var outcomeId in newOutcomesToAdd)
            {
                var matchOutcomeType = MatchOutcomeType.Create(
                    MatchOutcomeTypeId.Of(Guid.NewGuid()),
                    match.Id,
                    outcomeId
                );

                await context.MatchOutcomeTypes.AddAsync(matchOutcomeType, cancellationToken);
            }
        }

        public static async Task DeleteMatchOutcomeTypesAsync(this Match match, IApplicationDbContext context, CancellationToken cancellationToken)
        {
            // Fetch all MatchOutcomeTypes for the given match
            var matchOutcomeTypes = await context.MatchOutcomeTypes
                .Where(mot => mot.MatchId == match.Id)
                .ToListAsync(cancellationToken);

            if (matchOutcomeTypes.Any())
            {
                // Remove the outcomes
                context.MatchOutcomeTypes.RemoveRange(matchOutcomeTypes);
            }
        }


        public static IQueryable<MatchDto> ToMatchDtoList(this IQueryable<Match> matches, IQueryable<Team> teams, IQueryable<OutcomeType> outcomeTypes, IQueryable<MatchOutcomeType> matchOutcomeTypes)
        {
            return matches
                .Join(
                    teams,
                    match => match.Team1Id,
                    team => team.Id,
                    (match, team1) => new { match, team1 })
                .Join(
                    teams,
                    matchWithTeam1 => matchWithTeam1.match.Team2Id,
                    team => team.Id,
                    (matchWithTeam1, team2) => new { matchWithTeam1.match, matchWithTeam1.team1, team2 })
                .Select(mt => new MatchDto
                {
                    Id = mt.match.Id.Value,
                    Team1Id = mt.team1.Id.Value,
                    Team1Name = mt.team1.Name,
                    Team2Id = mt.team2.Id.Value,
                    Team2Name = mt.team2.Name,
                    Date = mt.match.Date,
                    Team1Goals = mt.match.Team1Goals,
                    Team2Goals = mt.match.Team2Goals,
                    IsCompleted = mt.match.IsCompleted,
                    OutcomeTypes = matchOutcomeTypes
                        .Where(mot => mot.MatchId == mt.match.Id)
                        .Join(
                            outcomeTypes,
                            mot => mot.OutcomeTypeId,
                            ot => ot.Id,
                            (mot, ot) => new OutcomeTypeDto
                            {
                                Id = ot.Id.Value,
                                Name = ot.Name,
                                Description = ot.Description
                            })
                        .ToList()
                });
        }

        public static MatchDto ToMatchDto(this Match match, IQueryable<Team> teams, IQueryable<OutcomeType> outcomeTypes, IQueryable<MatchOutcomeType> matchOutcomeTypes)
        {
            var team1 = teams.FirstOrDefault(t => t.Id == match.Team1Id);
            var team2 = teams.FirstOrDefault(t => t.Id == match.Team2Id);

            var outcomes = matchOutcomeTypes
                .Where(mot => mot.MatchId == match.Id)
                .Join(
                    outcomeTypes,
                    mot => mot.OutcomeTypeId,
                    ot => ot.Id,
                    (mot, ot) => new OutcomeTypeDto
                    {
                        Id = ot.Id.Value,
                        Name = ot.Name,
                        Description = ot.Description
                    })
                .ToList();

            return new MatchDto
            {
                Id = match.Id.Value,
                Team1Id = team1?.Id.Value ?? Guid.Empty,
                Team1Name = team1?.Name,
                Team2Id = team2?.Id.Value ?? Guid.Empty,
                Team2Name = team2?.Name,
                Date = match.Date,
                Team1Goals = match.Team1Goals,
                Team2Goals = match.Team2Goals,
                IsCompleted = match.IsCompleted,
                OutcomeTypes = outcomes
            };
        }
    }
}
