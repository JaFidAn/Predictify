namespace Prediction.Application.Features.Streaks.Queries.GetStreaksForTeam
{
    public record GetStreaksForTeamQuery(Guid TeamId) : IQuery<GetStreaksForTeamResult>;
    public record GetStreaksForTeamResult(Guid TeamId, string TeamName, Dictionary<string, StreakDto> Streaks);
}
