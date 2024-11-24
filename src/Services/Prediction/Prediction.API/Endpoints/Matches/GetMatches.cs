using Prediction.Application.Features.Matches.Queries.GetMatches;

namespace Prediction.API.Endpoints.Matches
{
    public record GetMatchesRequest(PaginationRequest PaginationRequest);
    public record GetMatchesResponse(PaginatedResult<MatchDto> Matches);

    public class GetMatches : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/matches", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetMatchesQuery(request));

                var response = result.Adapt<GetMatchesResponse>();

                return Results.Ok(response);
            })
           .WithName("GetMatches")
           .Produces<GetMatchesResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Matches")
           .WithDescription("Get Matches with Outcomes");
        }
    }
}
