using Prediction.Application.Features.Leagues.Queries.GetLeaguesByCountryId;

namespace Prediction.API.Endpoints.Leagues
{
    public record GetLeaguesByCountryIdRequest(Guid CountryId, PaginationRequest PaginationRequest);
    public record GetLeaguesByCountryIdResponse(PaginatedResult<LeagueDto> Leagues);

    public class GetLeaguesByCountryId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/leagues/countryId/{CountryId}", async (Guid countryId, [AsParameters] PaginationRequest paginationRequest, ISender sender) =>
            {
                var result = await sender.Send(new GetLeaguesByCountryIdQuery(countryId, paginationRequest));

                var response = result.Adapt<GetLeaguesByCountryIdResponse>();

                return Results.Ok(response);
            })
           .WithName("GetLeaguesByCountryId")
           .Produces<GetLeaguesByCountryIdResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Leagues By Country Id")
           .WithDescription("Get Leagues By Country Id");
        }
    }
}
