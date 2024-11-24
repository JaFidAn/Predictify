using Prediction.Application.Features.Leagues.Queries.GetLeagues;

namespace Prediction.API.Endpoints.Leagues
{
    public record GetLeaguesRequest(PaginationRequest PaginationRequest);
    public record GetLeaguesResponse(PaginatedResult<LeagueDto> Leagues);

    public class GetLeagues : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/leagues", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetLeaguesQuery(request));

                var response = result.Adapt<GetLeaguesResponse>();

                return Results.Ok(response);
            })
           .WithName("GetLeagues")
           .Produces<GetLeaguesResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Leagues")
           .WithDescription("Get Leagues");
        }
    }
}
