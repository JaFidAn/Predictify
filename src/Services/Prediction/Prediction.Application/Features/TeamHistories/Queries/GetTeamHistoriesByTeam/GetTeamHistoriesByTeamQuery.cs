namespace Prediction.Application.Features.TeamHistories.Queries.GetTeamHistoriesByTeam
{
    public record GetTeamHistoriesByTeamQuery(Guid TeamId, DateTime? StartDate = null, DateTime? EndDate = null) : IQuery<GetTeamHistoriesByTeamResult>;
    public record GetTeamHistoriesByTeamResult(List<TeamHistoryDto> Histories);
}
