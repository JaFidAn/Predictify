using Prediction.Application.Features.ForecastEvaluations.Queries.GetForecastEvaluations;

namespace Prediction.API.Endpoints.ForecastEvaluations
{
    public record GetForecastEvaluationsRequest(PaginationRequest PaginationRequest);
    public record GetForecastEvaluationsResponse(PaginatedResult<ForecastEvaluationDto> Evaluations);

    public class GetForecastEvaluations : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/evaluations", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetForecastEvaluationsQuery(request));

                var response = result.Adapt<GetForecastEvaluationsResponse>();

                return Results.Ok(response);
            })
           .WithName("GetForecastEvaluations")
           .Produces<GetForecastEvaluationsResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Paginated Forecast Evaluations")
           .WithDescription("Fetches Paginated Forecast Evaluations");
        }
    }
}
