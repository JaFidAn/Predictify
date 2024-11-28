using MediatR;

namespace Prediction.Application.Features.Imports.Commands.ImportMatches
{
    public record ImportMatchesCommand() : IRequest<Unit>;
}
