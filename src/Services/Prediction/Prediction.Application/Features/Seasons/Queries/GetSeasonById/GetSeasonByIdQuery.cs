namespace Prediction.Application.Features.Seasons.Queries.GetSeasonById
{
    public record GetSeasonByIdQuery(Guid Id) : IQuery<GetSeasonByIdResult>;
    public record GetSeasonByIdResult(SeasonDto Season);
}
