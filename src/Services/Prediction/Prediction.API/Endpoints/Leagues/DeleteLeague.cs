using Prediction.Application.Features.Leagues.Commands.DeleteLeague;

namespace Prediction.API.Endpoints.Leagues
{
    //public record DeleteLeagueRequest(Guid Id);
    public record DeleteLeagueResponse(bool IsSuccess);

    public class DeleteLeague : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/leagues/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteLeagueCommand(Id));

                var response = result.Adapt<DeleteLeagueResponse>();

                return Results.Ok(response);
            })
           .WithName("DeleteLeague")
           .Produces<DeleteLeagueResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Delete League")
           .WithDescription("Delete League");
        }
    }
}
