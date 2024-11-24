using Prediction.Application.Features.Matches.Queries.GetMatchesForTeam;

namespace Prediction.API.Endpoints.Matches
{
    public record GetMatchesForTeamRequest(Guid TeamId, PaginationRequest PaginationRequest);
    public record GetMatchesForTeamResponse(PaginatedResult<MatchDto> Matches);

    public class GetMatchesForTeam : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/matches/teamId/{TeamId}", async (Guid teamId, [AsParameters] PaginationRequest paginationRequest, ISender sender) =>
            {
                var result = await sender.Send(new GetMatchesForTeamQuery(teamId, paginationRequest));

                var response = result.Adapt<GetMatchesForTeamResponse>();

                return Results.Ok(response);
            })
           .WithName("GetMatchesForTeam")
           .Produces<GetMatchesForTeamResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Matches For Team")
           .WithDescription("Get All Matches By Team Id");
        }
    }
}
