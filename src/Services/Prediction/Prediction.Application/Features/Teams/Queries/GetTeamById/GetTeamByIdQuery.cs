namespace Prediction.Application.Features.Teams.Queries.GetTeamById
{
    public record GetTeamByIdQuery(Guid Id) : IQuery<GetTeamByIdResult>;
    public record GetTeamByIdResult(TeamDto Team);
}
