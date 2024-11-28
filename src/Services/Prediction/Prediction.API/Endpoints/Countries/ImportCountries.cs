using Prediction.Application.Features.Countries.Commands.ImportCountries;

namespace Prediction.API.Endpoints.Countries
{
    //public record ImportCountriesRequest(List<CountryApiDto> Countries);
    public record ImportCountriesResponse(bool IsImported);

    public class ImportCountries : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/countries/import", async (ISender sender) =>
            {
                var command = new ImportCountriesCommand();

                var result = await sender.Send(command);

                var response = new ImportCountriesResponse(result.IsImported);

                if (result.IsImported)
                {
                    return Results.Ok(response);
                }

                return Results.BadRequest(response);
            })
          .WithName("ImportCountries")
          .Produces<ImportCountriesResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Import Countries")
          .WithDescription("Fetch and import countries dynamically from an external API.");
        }
    }
}
