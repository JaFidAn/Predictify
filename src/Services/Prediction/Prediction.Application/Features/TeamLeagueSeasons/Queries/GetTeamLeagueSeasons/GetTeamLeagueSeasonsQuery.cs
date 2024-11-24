namespace Prediction.Application.Features.TeamLeagueSeasons.Queries.GetTeamLeagueSeasons
{
    public record GetTeamLeagueSeasonsQuery(PaginationRequest PaginationRequest) : IQuery<GetTeamLeagueSeasonsResult>;
    public record GetTeamLeagueSeasonsResult(PaginatedResult<TeamLeagueSeasonDto> TeamLeagueSeasons);
}
