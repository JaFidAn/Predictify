namespace Prediction.Application.Features.Teams.Queries.GetTeamsWithMaxStreaks
{
    public record GetTeamsWithMaxStreaksQuery(PaginationRequest PaginationRequest) : IQuery<GetTeamsWithMaxStreaksResult>;
    public record GetTeamsWithMaxStreaksResult(PaginatedResult<TeamsWithMaxStreaksDto> Teams);
}
