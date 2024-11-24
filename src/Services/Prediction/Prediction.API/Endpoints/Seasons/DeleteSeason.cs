using Prediction.Application.Features.Seasons.Commands.DeleteSeason;

namespace Prediction.API.Endpoints.Seasons
{
    //public record DeleteSeasonRequest(Guid Id);
    public record DeleteSeasonResponse(bool IsSuccess);

    public class DeleteSeason : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/seasons/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteSeasonCommand(Id));

                var response = result.Adapt<DeleteSeasonResponse>();

                return Results.Ok(response);
            })
           .WithName("DeleteSeason")
           .Produces<DeleteSeasonResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Delete Season")
           .WithDescription("Delete Season");
        }
    }
}
