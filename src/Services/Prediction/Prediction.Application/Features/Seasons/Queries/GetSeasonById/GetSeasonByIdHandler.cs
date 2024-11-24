namespace Prediction.Application.Features.Seasons.Queries.GetSeasonById
{
    public class GetSeasonByIdHandler(IApplicationDbContext context, ILogger<GetSeasonByIdHandler> logger) : IQueryHandler<GetSeasonByIdQuery, GetSeasonByIdResult>
    {
        public async Task<GetSeasonByIdResult> Handle(GetSeasonByIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetSeasonByIdHandler.Handle called with {@Query}", query);

            //get season by id using context
            var season = await context.Seasons
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == SeasonId.Of(query.Id), cancellationToken);

            if (season is null)
            {
                throw new ObjectNotFoundException(query.Id);
            }

            //return result
            return new GetSeasonByIdResult(season.ToSeasonDto());
        }
    }
}
