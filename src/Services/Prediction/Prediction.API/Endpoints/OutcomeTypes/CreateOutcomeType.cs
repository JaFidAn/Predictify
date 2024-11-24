using Prediction.Application.Features.OutcomeTypes.Commands.CreateOutcomeType;

namespace Prediction.API.Endpoints.OutcomeTypes
{
    public record CreateOutcomeTypeRequest(OutcomeTypeDto OutcomeType);
    public record CreateOutcomeTypeResponse(Guid Id);

    public class CreateOutcomeType : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/outcomeTypes", async (CreateOutcomeTypeRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateOutcomeTypeCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateOutcomeTypeResponse>();

                return Results.Created($"/outcomeTypes/{response.Id}", response);
            })
          .WithName("CreateOutcomeType")
          .Produces<CreateOutcomeTypeResponse>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Create Outcome Type")
          .WithDescription("Create Outcome Type");
        }
    }
}
