using Prediction.Application.Features.Imports.Commands.ImportMatches;

namespace Prediction.API.Endpoints.FootballDatas
{
    public class ImportMatches : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/import/matches", async (ISender sender) =>
            {
                try
                {
                    await sender.Send(new ImportMatchesCommand());
                    return Results.Ok("Football matches imported successfully.");
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: ex.Message,
                        statusCode: StatusCodes.Status500InternalServerError,
                        title: "An error occurred while importing football matches.");
                }
            })
            .WithName("ImportMatches")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Import Matches")
            .WithDescription("Imports matches and calculates match outcomes.");
        }
    }
}
