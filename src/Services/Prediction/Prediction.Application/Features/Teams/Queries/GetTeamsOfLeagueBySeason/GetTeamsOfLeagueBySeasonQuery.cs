namespace Prediction.Application.Features.Teams.Queries.GetTeamsOfLeagueBySeason
{
    public record GetTeamsOfLeagueBySeasonQuery(Guid LeagueId, Guid SeasonId) : IQuery<GetTeamsOfLeagueBySeasonResult>;
    public record GetTeamsOfLeagueBySeasonResult(IEnumerable<TeamsOfLeagueBySeasonDto> Teams);
}
