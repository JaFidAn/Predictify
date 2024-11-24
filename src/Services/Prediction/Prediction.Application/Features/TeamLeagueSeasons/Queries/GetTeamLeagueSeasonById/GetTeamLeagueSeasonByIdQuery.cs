namespace Prediction.Application.Features.TeamLeagueSeasons.Queries.GetTeamLeagueSeasonById
{
    public record GetTeamLeagueSeasonByIdQuery(Guid Id) : IQuery<GetTeamLeagueSeasonByIdResult>;
    public record GetTeamLeagueSeasonByIdResult(TeamLeagueSeasonDto TeamLeagueSeason);
}
