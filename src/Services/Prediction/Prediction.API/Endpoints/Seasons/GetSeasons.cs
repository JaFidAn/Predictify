using Prediction.Application.Features.Seasons.Queries.GetSeasons;

namespace Prediction.API.Endpoints.Seasons
{
    public record GetSeasonsRequest(PaginationRequest PaginationRequest);
    public record GetSeasonsResponse(PaginatedResult<SeasonDto> Seasons);

    public class GetSeasons : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/seasons", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetSeasonsQuery(request));

                var response = result.Adapt<GetSeasonsResponse>();

                return Results.Ok(response);
            })
          .WithName("GetSeasons")
           .Produces<GetSeasonsResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Seasons")
           .WithDescription("Get Seasons");
        }
    }
}
