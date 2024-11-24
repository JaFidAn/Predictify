using Prediction.Application.Features.Countries.Queries.GetCountryById;

namespace Prediction.API.Endpoints.Countries
{
    //public record GetCountryByIdRequest(Guid Id);
    public record GetCountryByIdResponse(CountryDto Country);

    public class GetCountryById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/countries/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetCountryByIdQuery(id));

                var response = result.Adapt<GetCountryByIdResponse>();

                return Results.Ok(response);
            })
            .WithName("GetCountryById")
            .Produces<GetCountryByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Country By Id")
            .WithDescription("Get Country By Id");
        }
    }
}
