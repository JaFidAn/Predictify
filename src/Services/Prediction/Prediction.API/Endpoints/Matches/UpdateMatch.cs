using Prediction.Application.Features.Matches.Commands.UpdateMatch;

namespace Prediction.API.Endpoints.Matches
{
    public record UpdateMatchRequest(MatchDto Match);
    public record UpdateMatchResponse(bool IsSuccess);

    public class UpdateMatch : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/matches", async (UpdateMatchRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateMatchCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateMatchResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateMatch")
            .Produces<UpdateMatchResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Match")
            .WithDescription("Update Match");
        }
    }
}
