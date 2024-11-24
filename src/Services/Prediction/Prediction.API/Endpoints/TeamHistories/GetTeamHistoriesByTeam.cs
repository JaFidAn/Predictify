using Prediction.Application.Features.TeamHistories.Queries.GetTeamHistoriesByTeam;

namespace Prediction.API.Endpoints.TeamHistories
{
    //public record GetTeamHistoriesByTeamRequest(Guid teamId, DateTime? startDate, DateTime? endDate);
    public record GetTeamHistoriesByTeamResponse(List<TeamHistoryDto> Histories);

    public class GetTeamHistoriesByTeam : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/team-histories/team/{teamId}", async (Guid teamId, DateTime? startDate, DateTime? endDate, ISender sender) =>
            {
                // Send query to get team histories
                var result = await sender.Send(new GetTeamHistoriesByTeamQuery(teamId, startDate, endDate));

                // Adapt result to response
                var response = new GetTeamHistoriesByTeamResponse(result.Histories);

                return Results.Ok(response);
            })
            .WithName("GetTeamHistoriesByTeam")
            .Produces<GetTeamHistoriesByTeamResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Team History by Team")
            .WithDescription("Fetch the history of a team, optionally filtered by date range, sorted in descending order by date.");
        }
    }
}
