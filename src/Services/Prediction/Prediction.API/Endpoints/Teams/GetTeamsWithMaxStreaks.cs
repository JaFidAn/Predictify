using Prediction.Application.Features.Teams.Queries.GetTeamsWithMaxStreaks;

namespace Prediction.API.Endpoints.Teams
{
    public record GetTeamsWithMaxStreaksRequest(PaginationRequest PaginationRequest);
    public record GetTeamsWithMaxStreaksResponse(PaginatedResult<TeamsWithMaxStreaksDto> Teams);

    public class GetTeamsWithMaxStreaks : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teams/max-streaks", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetTeamsWithMaxStreaksQuery(request));

                var response = result.Adapt<GetTeamsWithMaxStreaksResponse>();

                return Results.Ok(response);
            })
           .WithName("GetTeamsWithMaxStreaks")
           .Produces<GetTeamsWithMaxStreaksResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Teams with Maximum Streaks")
           .WithDescription("Get Teams with Maximum Streaks");
        }
    }
}
