using Prediction.Application.Features.TeamLeagueSeasons.Commands.CreateTeamLeagueSeason;

namespace Prediction.API.Endpoints.TeamLeagueSeasons
{
    public record CreateTeamLeagueSeasonRequest(TeamLeagueSeasonDto TeamLeagueSeason);
    public record CreateTeamLeagueSeasonResponse(Guid Id);

    public class CreateTeamLeagueSeason : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/team-league-seasons", async (CreateTeamLeagueSeasonRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateTeamLeagueSeasonCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateTeamLeagueSeasonResponse>();

                return Results.Created($"/team-league-seasons/{response.Id}", response);
            })
          .WithName("CreateTeamLeagueSeason")
          .Produces<CreateTeamLeagueSeasonResponse>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Create TeamLeagueSeason")
          .WithDescription("Create TeamLeagueSeason");
        }
    }
}
