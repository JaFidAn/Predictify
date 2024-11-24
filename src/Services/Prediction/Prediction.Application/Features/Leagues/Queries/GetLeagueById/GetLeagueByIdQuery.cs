namespace Prediction.Application.Features.Leagues.Queries.GetLeagueById
{
    public record GetLeagueByIdQuery(Guid Id) : IQuery<GetLeagueByIdResult>;
    public record GetLeagueByIdResult(LeagueDto League);
}
