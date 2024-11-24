using Prediction.Application.Features.Teams.Queries.GetTeamsOfLeagueBySeason;

namespace Prediction.API.Endpoints.Teams
{
    public record GetTeamsOfLeagueBySeasonRequest(Guid LeagueId, Guid SeasonId);
    public record GetTeamsOfLeagueBySeasonResponse(IEnumerable<TeamsOfLeagueBySeasonDto> Teams);

    public class GetTeamsOfLeagueBySeason : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/teams/leagueId/{LeagueId}/seasonId/{SeasonId}", async (Guid leagueId, Guid seasonId, ISender sender) =>
            {
                var result = await sender.Send(new GetTeamsOfLeagueBySeasonQuery(leagueId, seasonId));

                var response = result.Adapt<GetTeamsOfLeagueBySeasonResponse>();

                return Results.Ok(response);
            })
           .WithName("GetTeamsOfLeagueBySeason")
           .Produces<GetTeamsOfLeagueBySeasonResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Teams of a League by Season")
           .WithDescription("Retrieve all teams associated with a specific league and season.");
        }
    }
}
