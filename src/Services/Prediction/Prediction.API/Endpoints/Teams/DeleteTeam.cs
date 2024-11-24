using Prediction.Application.Features.Teams.Commands.DeleteTeam;

namespace Prediction.API.Endpoints.Teams
{
    //public record DeleteTeamRequest(Guid Id);
    public record DeleteTeamResponse(bool IsSuccess);

    public class DeleteTeam : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/teams/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteTeamCommand(Id));

                var response = result.Adapt<DeleteTeamResponse>();

                return Results.Ok(response);
            })
           .WithName("DeleteTeam")
           .Produces<DeleteTeamResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Delete Team")
           .WithDescription("Delete Team");
        }
    }
}
