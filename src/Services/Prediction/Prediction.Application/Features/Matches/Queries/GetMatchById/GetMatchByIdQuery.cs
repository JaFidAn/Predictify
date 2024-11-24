namespace Prediction.Application.Features.Matches.Queries.GetMatchById
{
    public record GetMatchByIdQuery(Guid Id) : IQuery<GetMatchByIdResult>;
    public record GetMatchByIdResult(MatchDto Match);
}
