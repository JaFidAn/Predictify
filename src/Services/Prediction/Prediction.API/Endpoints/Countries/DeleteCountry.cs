using Prediction.Application.Features.Countries.Commands.DeleteCountry;

namespace Prediction.API.Endpoints.Countries
{
    //public record DeleteCountryRequest(Guid Id);
    public record DeleteCountryResponse(bool IsSuccess);

    public class DeleteCountry : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/countries/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteCountryCommand(Id));

                var response = result.Adapt<DeleteCountryResponse>();

                return Results.Ok(response);
            })
           .WithName("DeleteCountry")
           .Produces<DeleteCountryResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Delete Country")
           .WithDescription("Delete Country");
        }
    }
}
