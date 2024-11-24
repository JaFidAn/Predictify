namespace Prediction.Application.Features.Matches.Queries.GetMatchesForTeam
{
    public record GetMatchesForTeamQuery(Guid TeamId, PaginationRequest PaginationRequest) : IQuery<GetMatchesForTeamResult>;
    public record GetMatchesForTeamResult(PaginatedResult<MatchDto> Matches);
}
