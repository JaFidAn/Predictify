namespace Prediction.Application.Features.Matches.Queries.GetMatches
{
    public record GetMatchesQuery(PaginationRequest PaginationRequest) : IQuery<GetMatchesResult>;
    public record GetMatchesResult(PaginatedResult<MatchDto> Matches);
}
