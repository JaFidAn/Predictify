using Prediction.Application.Features.Countries.Queries.GetCountries;

namespace Prediction.API.Endpoints.Countries
{
    public record GetCountriesRequest(PaginationRequest PaginationRequest);
    public record GetCountriesResponse(PaginatedResult<CountryDto> Countries);

    public class GetCountries : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/countries", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetCountriesQuery(request));

                var response = result.Adapt<GetCountriesResponse>();

                return Results.Ok(response);
            })
          .WithName("GetCountries")
           .Produces<GetCountriesResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Countries")
           .WithDescription("Get Countries");
        }
    }
}
