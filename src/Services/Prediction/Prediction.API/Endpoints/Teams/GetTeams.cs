using Prediction.Application.Features.Teams.Queries.GetTeams;

namespace Prediction.API.Endpoints.Teams
{
    public record GetTeamsRequest(PaginationRequest PaginationRequest);
    public record GetTeamsResponse(PaginatedResult<TeamDto> Teams);

    public class GetTeams : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teams", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetTeamsQuery(request));

                var response = result.Adapt<GetTeamsResponse>();

                return Results.Ok(response);
            })
          .WithName("GetTeams")
           .Produces<GetTeamsResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Teams")
           .WithDescription("Get Teams");
        }
    }
}
