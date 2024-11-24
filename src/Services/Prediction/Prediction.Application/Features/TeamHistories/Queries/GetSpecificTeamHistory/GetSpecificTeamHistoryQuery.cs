namespace Prediction.Application.Features.TeamHistories.Queries.GetSpecificTeamHistory
{
    public record GetSpecificTeamHistoryQuery(Guid TeamHistoryId) : IQuery<GetSpecificTeamHistoryResult>;

    public record GetSpecificTeamHistoryResult(TeamHistoryDto TeamHistory);
}
