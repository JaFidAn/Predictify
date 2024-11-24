namespace Prediction.Application.Features.Teams.Queries.GetTeamById
{
    public class GetTeamByIdHandler(IApplicationDbContext context, ILogger<GetTeamByIdHandler> logger) : IQueryHandler<GetTeamByIdQuery, GetTeamByIdResult>
    {
        public async Task<GetTeamByIdResult> Handle(GetTeamByIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetTeamByIdHandler.Handle called with {@Query}", query);

            //get team by id using context
            var team = await context.Teams
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == TeamId.Of(query.Id), cancellationToken);

            if (team is null)
            {
                throw new ObjectNotFoundException(query.Id);
            }

            //return result
            return new GetTeamByIdResult(team.ToTeamDto(context.Leagues, context.Countries));
        }
    }
}
