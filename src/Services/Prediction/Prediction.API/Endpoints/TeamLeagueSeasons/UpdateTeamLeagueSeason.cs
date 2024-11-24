using Prediction.Application.Features.TeamLeagueSeasons.Commands.UpdateTeamLeagueSeason;

namespace Prediction.API.Endpoints.TeamLeagueSeasons
{
    public record UpdateTeamLeagueSeasonRequest(TeamLeagueSeasonDto TeamLeagueSeason);
    public record UpdateTeamLeagueSeasonResponse(bool IsSuccess);

    public class UpdateTeamLeagueSeason : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/team-league-seasons", async (UpdateTeamLeagueSeasonRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateTeamLeagueSeasonCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateTeamLeagueSeasonResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateTeamLeagueSeason")
            .Produces<UpdateTeamLeagueSeasonResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update TeamLeagueSeason")
            .WithDescription("Update TeamLeagueSeason");
        }
    }
}
