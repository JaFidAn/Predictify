using Prediction.Application.Features.Seasons.Commands.UpdateSeason;

namespace Prediction.API.Endpoints.Seasons
{
    public record UpdateSeasonRequest(SeasonDto Season);
    public record UpdateSeasonResponse(bool IsSuccess);

    public class UpdateSeason : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/seasons", async (UpdateSeasonRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateSeasonCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateSeasonResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateSeason")
            .Produces<UpdateSeasonResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Season")
            .WithDescription("Update Season");
        }
    }
}
