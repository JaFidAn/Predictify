using Prediction.Application.Features.TeamHistories.Queries.GetSpecificTeamHistory;

namespace Prediction.API.Endpoints.TeamHistories
{
    //public record GetSpecificTeamHistoryRequest(Guid TeamHistoryId);
    public record GetSpecificTeamHistoryResponse(TeamHistoryDto TeamHistory);

    public class GetSpecificTeamHistory : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/team-histories/{TeamHistoryId}", async (Guid teamHistoryId, ISender sender) =>
            {
                var result = await sender.Send(new GetSpecificTeamHistoryQuery(teamHistoryId));

                var response = result.Adapt<GetSpecificTeamHistoryResponse>();

                return Results.Ok(response);
            })
           .WithName("GetSpecificTeamHistory")
           .Produces<GetSpecificTeamHistoryResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Specific Team History")
           .WithDescription("Retrieve details of a specific Team History record by ID.");
        }
    }
}
