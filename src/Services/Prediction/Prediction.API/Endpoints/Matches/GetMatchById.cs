using Prediction.Application.Features.Matches.Queries.GetMatchById;

namespace Prediction.API.Endpoints.Matches
{
    //public record GetMatchByIdRequest(Guid Id);
    public record GetMatchByIdResponse(MatchDto Match);

    public class GetMatchById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/matches/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetMatchByIdQuery(id));

                var response = result.Adapt<GetMatchByIdResponse>();

                return Results.Ok(response);
            })
            .WithName("GetMatchById")
            .Produces<GetMatchByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Match By Id")
            .WithDescription("Get Match By Id");
        }
    }
}
