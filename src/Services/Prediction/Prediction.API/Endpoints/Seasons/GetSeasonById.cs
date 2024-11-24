using Prediction.Application.Features.Seasons.Queries.GetSeasonById;

namespace Prediction.API.Endpoints.Seasons
{
    //public record GetSeasonByIdRequest(Guid Id);
    public record GetSeasonByIdResponse(SeasonDto Season);

    public class GetSeasonById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/seasons/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetSeasonByIdQuery(id));

                var response = result.Adapt<GetSeasonByIdResponse>();

                return Results.Ok(response);
            })
            .WithName("GetSeasonById")
            .Produces<GetSeasonByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Season By Id")
            .WithDescription("Get Season By Id");
        }
    }
}
