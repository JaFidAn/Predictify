namespace Prediction.Application.Features.TeamHistories.Commands.RecalculateTeamHistories
{
    public record RecalculateTeamHistoriesAndStreaksCommand : ICommand<RecalculateTeamHistoriesAndStreaksResult>;
    public record RecalculateTeamHistoriesAndStreaksResult(string Message, int UpdatedTeamsCount);
}
