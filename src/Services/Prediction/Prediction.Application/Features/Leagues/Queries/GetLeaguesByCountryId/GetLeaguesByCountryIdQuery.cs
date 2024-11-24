namespace Prediction.Application.Features.Leagues.Queries.GetLeaguesByCountryId
{
    public record GetLeaguesByCountryIdQuery(Guid CountryId, PaginationRequest PaginationRequest) : IQuery<GetLeaguesByCountryIdResult>;
    public record GetLeaguesByCountryIdResult(PaginatedResult<LeagueDto> Leagues);
}
