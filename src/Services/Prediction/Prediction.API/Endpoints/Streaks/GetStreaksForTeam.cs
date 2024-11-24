using Prediction.Application.Features.Streaks.Queries.GetStreaksForTeam;

namespace Prediction.API.Endpoints.Streaks
{
    //public record GetStreaksForTeamRequest(Guid TeamId);
    public record GetStreaksForTeamResponse(Guid TeamId, string TeamName, Dictionary<string, StreakDto> Streaks);

    public class GetStreaksForTeam : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/streaks/teamId/{TeamId}", async (Guid teamId, ISender sender) =>
            {
                var result = await sender.Send(new GetStreaksForTeamQuery(teamId));

                var response = result.Adapt<GetStreaksForTeamResponse>();

                return Results.Ok(response);
            })
           .WithName("GetStreaksForTeam")
           .Produces<GetStreaksForTeamResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Streaks For Team")
           .WithDescription("Get Streaks For Team");
        }
    }
}
