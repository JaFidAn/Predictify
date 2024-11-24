using Prediction.Application.Features.TeamLeagueSeasons.Commands.DeleteTeamLeagueSeason;

namespace Prediction.API.Endpoints.TeamLeagueSeasons
{
    //public record DeleteTeamLeagueSeasonRequest(Guid Id);
    public record DeleteTeamLeagueSeasonResponse(bool IsSuccess);

    public class DeleteTeamLeagueSeason : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/team-league-seasons/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteTeamLeagueSeasonCommand(Id));

                var response = result.Adapt<DeleteTeamLeagueSeasonResponse>();

                return Results.Ok(response);
            })
           .WithName("DeleteTeamLeagueSeason")
           .Produces<DeleteTeamLeagueSeasonResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Delete TeamLeagueSeason")
           .WithDescription("Delete TeamLeagueSeason");
        }
    }
}
