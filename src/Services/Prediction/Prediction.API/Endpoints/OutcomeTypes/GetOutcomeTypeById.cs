using Prediction.Application.Features.OutcomeTypes.Queries.GetOutcomeTypeById;

namespace Prediction.API.Endpoints.OutcomeTypes
{
    //public record GetOutcomeTypeByIdRequest(Guid Id);
    public record GetOutcomeTypeByIdResponse(OutcomeTypeDto OutcomeType);

    public class GetOutcomeTypeById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/outcomeTypes/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetOutcomeTypeByIdQuery(id));

                var response = result.Adapt<GetOutcomeTypeByIdResponse>();

                return Results.Ok(response);
            })
            .WithName("GetOutcomeTypeById")
            .Produces<GetOutcomeTypeByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Outcome Type By Id")
            .WithDescription("Get Outcome Type By Id");
        }
    }
}
