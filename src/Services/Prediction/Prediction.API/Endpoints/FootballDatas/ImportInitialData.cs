using Prediction.Application.Features.Imports.Commands.ImportInitialData;

namespace Prediction.API.Endpoints.FootballDatas
{
    public class ImportInitialData : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/import/initial-data", async (ISender sender) =>
            {
                try
                {
                    await sender.Send(new ImportInitialDataCommand());
                    return Results.Ok("Initial football data imported successfully.");
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: ex.Message,
                        statusCode: StatusCodes.Status500InternalServerError,
                        title: "An error occurred while importing initial football data.");
                }
            })
            .WithName("ImportInitialData")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Import Initial Data")
            .WithDescription("Imports countries, leagues, seasons, teams, and team-league-season relationships.");
        }
    }
}
