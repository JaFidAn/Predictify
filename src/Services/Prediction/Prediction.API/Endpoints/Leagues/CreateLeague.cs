using Prediction.Application.Features.Leagues.Commands.CreateLeague;

namespace Prediction.API.Endpoints.Leagues
{
    public record CreateLeagueRequest(LeagueDto League);
    public record CreateLeagueResponse(Guid Id);

    public class CreateLeague : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/leagues", async (CreateLeagueRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateLeagueCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateLeagueResponse>();

                return Results.Created($"/leagues/{response.Id}", response);
            })
          .WithName("CreateLeague")
          .Produces<CreateLeagueResponse>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Create League")
          .WithDescription("Create League");
        }
    }
}
