using Prediction.Application.Features.OutcomeTypes.Queries.GetOutcomeTypes;

namespace Prediction.API.Endpoints.OutcomeTypes
{
    public record GetOutcomeTypesRequest(PaginationRequest PaginationRequest);
    public record GetOutcomeTypesResponse(PaginatedResult<OutcomeTypeDto> OutcomeTypes);

    public class GetOutcomeTypes : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/outcomeTypes", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetOutcomeTypesQuery(request));

                var response = result.Adapt<GetOutcomeTypesResponse>();

                return Results.Ok(response);
            })
          .WithName("GetOutcomeTypes")
           .Produces<GetOutcomeTypesResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Outcome Types")
           .WithDescription("Get Outcome Types");
        }
    }
}
