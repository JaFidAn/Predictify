using Prediction.Application.Features.Teams.Queries.GetTeamsByLeagueId;

namespace Prediction.API.Endpoints.Teams
{
    public record GetTeamsByLeagueIdRequest(Guid LeagueId, PaginationRequest PaginationRequest);
    public record GetTeamsByLeagueIdResponse(PaginatedResult<TeamDto> Teams);

    public class GetTeamsByLeagueId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teams/leagueId/{LeagueId}", async (Guid leagueId, [AsParameters] PaginationRequest paginationRequest, ISender sender) =>
            {
                var result = await sender.Send(new GetTeamsByLeagueIdQuery(leagueId, paginationRequest));

                var response = result.Adapt<GetTeamsByLeagueIdResponse>();

                return Results.Ok(response);
            })
           .WithName("GetTeamsByLeagueId")
           .Produces<GetTeamsByLeagueIdResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Teams By League Id")
           .WithDescription("Get Teams By League Id");
        }
    }
}
