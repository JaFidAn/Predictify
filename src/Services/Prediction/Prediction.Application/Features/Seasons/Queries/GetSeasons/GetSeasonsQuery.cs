namespace Prediction.Application.Features.Seasons.Queries.GetSeasons
{
    public record GetSeasonsQuery(PaginationRequest PaginationRequest) : IQuery<GetSeasonsResult>;
    public record GetSeasonsResult(PaginatedResult<SeasonDto> Seasons);
}
