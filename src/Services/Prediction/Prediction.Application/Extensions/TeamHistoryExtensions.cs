namespace Prediction.Application.Extensions
{
    public static class TeamHistoryExtensions
    {
        public static async Task UpdateTeamHistoriesAsync(this TeamId teamId, IApplicationDbContext context, CancellationToken cancellationToken)
        {
            // Fetch all matches involving the team
            var matches = await context.Matches
                .Where(m => m.Team1Id == teamId || m.Team2Id == teamId)
                .OrderBy(m => m.Date)
                .ToListAsync(cancellationToken);

            if (!matches.Any())
                return;

            // Remove existing histories for the team
            var existingHistories = await context.TeamHistories
                .Where(th => th.TeamId == teamId)
                .ToListAsync(cancellationToken);
            context.TeamHistories.RemoveRange(existingHistories);

            // Fetch all OutcomeTypes
            var outcomeTypes = await context.OutcomeTypes.ToListAsync(cancellationToken);

            var newHistories = new List<TeamHistory>();

            // Recalculate and add new histories
            foreach (var match in matches)
            {
                var outcomesForTeam = await match.GetApplicableOutcomesAsync(context, teamId, cancellationToken);

                foreach (var outcome in outcomesForTeam)
                {
                    var isTeam1 = match.Team1Id == teamId;

                    var teamHistory = TeamHistory.Create(
                        TeamHistoryId.Of(Guid.NewGuid()),
                        teamId,
                        isTeam1 ? match.Team2Id : match.Team1Id,
                        match.Date,
                        isTeam1 ? match.Team1Goals ?? 0 : match.Team2Goals ?? 0,
                        isTeam1 ? match.Team2Goals ?? 0 : match.Team1Goals ?? 0,
                        outcome
                    );

                    newHistories.Add(teamHistory);
                }
            }

            // Add new histories to the database
            context.TeamHistories.AddRange(newHistories);

            // Save changes
            await context.SaveChangesAsync(cancellationToken);
        }
    

        public static IQueryable<TeamHistoryDto> ToTeamHistoryDtoList(this IQueryable<TeamHistory> teamHistories,IQueryable<Team> teams, IQueryable<OutcomeType> outcomeTypes)
        {
            return teamHistories
                .Join(
                    teams,
                    history => history.OpponentId,
                    team => team.Id,
                    (history, opponent) => new { history, opponent })
                .Join(
                    outcomeTypes,
                    historyWithOpponent => historyWithOpponent.history.OutcomeTypeId,
                    outcome => outcome.Id,
                    (historyWithOpponent, outcome) => new TeamHistoryDto
                    {
                        Id = historyWithOpponent.history.Id.Value,
                        TeamId = historyWithOpponent.history.TeamId.Value,
                        TeamName = teams.First(t => t.Id == historyWithOpponent.history.TeamId).Name,
                        OpponentId = historyWithOpponent.opponent.Id.Value,
                        OpponentName = historyWithOpponent.opponent.Name,
                        Date = historyWithOpponent.history.Date,
                        GoalsScored = historyWithOpponent.history.GoalsScored,
                        GoalsConceded = historyWithOpponent.history.GoalsConceded,
                        OutcomeTypeId = historyWithOpponent.history.OutcomeTypeId.Value,
                        OutcomeTypeName = outcome.Name
                    });
        }
    }
}
