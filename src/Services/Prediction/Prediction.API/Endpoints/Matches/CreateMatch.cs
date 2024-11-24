using Prediction.Application.Features.Matches.Commands.CreateMatch;

namespace Prediction.API.Endpoints.Matches
{
    public record CreateMatchRequest(MatchDto Match);
    public record CreateMatchResponse(Guid Id);

    public class CreateMatch : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/matches", async (CreateMatchRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateMatchCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateMatchResponse>();

                return Results.Created($"/matches/{response.Id}", response);
            })
          .WithName("CreateMatch")
          .Produces<CreateMatchResponse>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Create Match")
          .WithDescription("Create Match");
        }
    }
}
