using Prediction.Application.Features.Countries.Commands.UpdateCountry;

namespace Prediction.API.Endpoints.Countries
{
    public record UpdateCountryRequest(CountryDto Country);
    public record UpdateCountryResponse(bool IsSuccess);

    public class UpdateCountry : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/countries", async (UpdateCountryRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateCountryCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateCountryResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateCountry")
            .Produces<UpdateCountryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Country")
            .WithDescription("Update Country");
        }
    }
}
