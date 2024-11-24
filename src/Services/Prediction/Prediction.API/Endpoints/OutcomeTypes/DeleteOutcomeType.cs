using Prediction.Application.Features.OutcomeTypes.Commands.DeleteOutcomeType;

namespace Prediction.API.Endpoints.OutcomeTypes
{
    //public record DeleteOutcomeTypeRequest(Guid Id);
    public record DeleteOutcomeTypeResponse(bool IsSuccess);

    public class DeleteOutcomeType : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/outcomeTypes/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteOutcomeTypeCommand(Id));

                var response = result.Adapt<DeleteOutcomeTypeResponse>();

                return Results.Ok(response);
            })
           .WithName("DeleteOutcomeType")
           .Produces<DeleteOutcomeTypeResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Delete Outcome Type")
           .WithDescription("Delete Outcome Type");
        }
    }
}
