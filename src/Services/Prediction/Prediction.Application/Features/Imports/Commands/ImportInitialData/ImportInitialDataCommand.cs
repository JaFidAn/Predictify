using MediatR;

namespace Prediction.Application.Features.Imports.Commands.ImportInitialData
{
    public record ImportInitialDataCommand() : IRequest<Unit>;
}
