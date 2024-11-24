namespace Prediction.Application.Features.Leagues.Queries.GetLeagues
{
    public record GetLeaguesQuery(PaginationRequest PaginationRequest) : IQuery<GetLeaguesResult>;
    public record GetLeaguesResult(PaginatedResult<LeagueDto> Leagues);
}
