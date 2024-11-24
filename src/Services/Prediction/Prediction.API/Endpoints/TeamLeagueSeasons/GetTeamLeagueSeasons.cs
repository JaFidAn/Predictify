using Prediction.Application.Features.TeamLeagueSeasons.Queries.GetTeamLeagueSeasons;

namespace Prediction.API.Endpoints.TeamLeagueSeasons
{
    public record GetTeamLeagueSeasonsRequest(PaginationRequest PaginationRequest);
    public record GetTeamLeagueSeasonsResponse(PaginatedResult<TeamLeagueSeasonDto> TeamLeagueSeasons);

    public class GetTeamLeagueSeasons : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/team-league-seasons", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetTeamLeagueSeasonsQuery(request));

                var response = result.Adapt<GetTeamLeagueSeasonsResponse>();

                return Results.Ok(response);
            })
          .WithName("GetTeamLeagueSeasons")
           .Produces<GetTeamLeagueSeasonsResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get TeamLeagueSeasons")
           .WithDescription("Get TeamLeagueSeasons");
        }
    }
}
