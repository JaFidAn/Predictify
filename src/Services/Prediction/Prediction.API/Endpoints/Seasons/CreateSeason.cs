using Prediction.Application.Features.Seasons.Commands.CreateSeason;

namespace Prediction.API.Endpoints.Seasons
{
    public record CreateSeasonRequest(SeasonDto Season);
    public record CreateSeasonResponse(Guid Id);

    public class CreateSeason : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/seasons", async (CreateSeasonRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateSeasonCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateSeasonResponse>();

                return Results.Created($"/seasons/{response.Id}", response);
            })
          .WithName("CreateSeason")
          .Produces<CreateSeasonResponse>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Create Season")
          .WithDescription("Create Season");
        }
    }
}
