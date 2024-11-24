using Prediction.Application.Features.OutcomeTypes.Commands.UpdateOutcomeType;

namespace Prediction.API.Endpoints.OutcomeTypes
{
    public record UpdateOutcomeTypeRequest(OutcomeTypeDto OutcomeType);
    public record UpdateOutcomeTypeResponse(bool IsSuccess);

    public class UpdateOutcomeType : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/outcomeTypes", async (UpdateOutcomeTypeRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateOutcomeTypeCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateOutcomeTypeResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateOutcomeType")
            .Produces<UpdateOutcomeTypeResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Outcome Type")
            .WithDescription("Update Outcome Type");
        }
    }
}
