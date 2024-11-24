using Prediction.Application.Features.Leagues.Queries.GetLeagueById;

namespace Prediction.API.Endpoints.Leagues
{
    //public record GetLeagueByIdRequest(Guid Id);
    public record GetLeagueByIdResponse(LeagueDto League);

    public class GetLeagueById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/leagues/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetLeagueByIdQuery(id));

                var response = result.Adapt<GetLeagueByIdResponse>();

                return Results.Ok(response);
            })
            .WithName("GetLeagueById")
            .Produces<GetLeagueByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get League By Id")
            .WithDescription("Get League By Id");
        }
    }
}
