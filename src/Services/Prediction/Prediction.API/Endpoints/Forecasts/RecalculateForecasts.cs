using Prediction.Application.Features.Forecasts.Commands.RecalculateForecasts;

namespace Prediction.API.Endpoints.Forecasts
{
    public record RecalculateForecastsRequest(Guid? MatchId);
    public record RecalculateForecastsResponse(int ForecastsRecalculated);

    public class RecalculateForecasts : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/forecasts/recalculate", async (RecalculateForecastsRequest request, ISender sender) =>
            {
                var command = new RecalculateForecastsCommand(request.MatchId);

                var result = await sender.Send(command);

                var response = result.Adapt<RecalculateForecastsResponse>();

                return Results.Ok(response);
            })
          .WithName("RecalculateForecasts")
          .Produces<RecalculateForecastsResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Recalculate Forecasts")
          .WithDescription("Recalculate forecasts for all matches or a specific match based on MatchId.");
        }
    }
}
