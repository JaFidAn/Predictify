using Prediction.Application.Features.Leagues.Commands.UpdateLeague;

namespace Prediction.API.Endpoints.Leagues
{
    public record UpdateLeagueRequest(LeagueDto League);
    public record UpdateLeagueResponse(bool IsSuccess);

    public class UpdateLeague : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/leagues", async (UpdateLeagueRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateLeagueCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateLeagueResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateLeague")
            .Produces<UpdateLeagueResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update League")
            .WithDescription("Update League");
        }
    }
}
