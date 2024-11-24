namespace Prediction.Application.Features.Leagues.Queries.GetLeagueById
{
    public class GetLeagueByIdHandler(IApplicationDbContext context, ILogger<GetLeagueByIdHandler> logger) : IQueryHandler<GetLeagueByIdQuery, GetLeagueByIdResult>
    {
        public async Task<GetLeagueByIdResult> Handle(GetLeagueByIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetLeagueByIdHandler.Handle called with {@Query}", query);

            //get league by id using context
            var league = await context.Leagues
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == LeagueId.Of(query.Id), cancellationToken);

            if (league is null)
            {
                throw new ObjectNotFoundException(query.Id);
            }

            //return result
            return new GetLeagueByIdResult(league.ToLeagueDto(context.Countries));
        }
    }
}
