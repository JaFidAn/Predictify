using Prediction.Application.Features.Teams.Queries.GetTeamById;

namespace Prediction.API.Endpoints.Teams
{
    //public record GetTeamByIdRequest(Guid Id);
    public record GetTeamByIdResponse(TeamDto Team);

    public class GetTeamById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teams/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetTeamByIdQuery(id));

                var response = result.Adapt<GetTeamByIdResponse>();

                return Results.Ok(response);
            })
            .WithName("GetTeamById")
            .Produces<GetTeamByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Team By Id")
            .WithDescription("Get Team By Id");
        }
    }
}
