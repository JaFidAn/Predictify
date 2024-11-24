using Prediction.Application.Features.TeamHistories.Commands.RecalculateTeamHistories;

namespace Prediction.API.Endpoints.TeamHistories
{
    //public record RecalculateTeamHistoriesAndStreaksRequest();
    public record RecalculateTeamHistoriesAndStreaksResponse(string Message, int UpdatedTeamsCount);

    public class RecalculateTeamHistories : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/team-histories/recalculate", async (ISender sender) =>
            {
                var command = new RecalculateTeamHistoriesAndStreaksCommand();

                var result = await sender.Send(command);

                var response = result.Adapt<RecalculateTeamHistoriesAndStreaksResponse>();

                return Results.Ok(response);
            })
          .WithName("RecalculateTeamHistoriesAndStreaks")
          .Produces<RecalculateTeamHistoriesAndStreaksResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Recalculate Team Histories and Streaks for All Teams")
          .WithDescription("Recalculates team histories and Streaks for all teams based on previous matches.");
        }
    }
}
