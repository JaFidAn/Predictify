namespace Prediction.Application.Features.Teams.Queries.GetTeamsByLeagueId
{
    public record GetTeamsByLeagueIdQuery(Guid LeagueId, PaginationRequest PaginationRequest) : IQuery<GetTeamsByLeagueIdResult>;
    public record GetTeamsByLeagueIdResult(PaginatedResult<TeamDto> Teams);
}
