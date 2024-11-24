using Prediction.Application.Features.Forecasts.Queries.GetForecasts;

namespace Prediction.API.Endpoints.Forecasts
{
    public record GetForecastsRequest(PaginationRequest PaginationRequest);
    public record GetForecastsResponse(PaginatedResult<ForecastDto> Forecasts);

    public class GetForecasts : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/forecasts", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetForecastsQuery(request));

                var response = result.Adapt<GetForecastsResponse>();

                return Results.Ok(response);
            })
           .WithName("GetForecasts")
           .Produces<GetForecastsResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Forecasts")
           .WithDescription("Get All Forecast with Pagination");
        }
    }
}
