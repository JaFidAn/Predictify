using Prediction.Application.Features.TeamLeagueSeasons.Queries.GetTeamLeagueSeasonById;

namespace Prediction.API.Endpoints.TeamLeagueSeasons
{
    //public record GetTeamLeagueSeasonByIdRequest(Guid Id);
    public record GetTeamLeagueSeasonByIdResponse(TeamLeagueSeasonDto TeamLeagueSeason);

    public class GetTeamLeagueSeasonById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/team-league-seasons/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetTeamLeagueSeasonByIdQuery(id));

                var response = result.Adapt<GetTeamLeagueSeasonByIdResponse>();

                return Results.Ok(response);
            })
            .WithName("GetTeamLeagueSeasonById")
            .Produces<GetTeamLeagueSeasonByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get TeamLeagueSeason By Id")
            .WithDescription("Get TeamLeagueSeason By Id");
        }
    }
}
