namespace Prediction.Application.Features.Forecasts.Queries.GetForecasts
{
    public class GetForecastsHandler(IApplicationDbContext context, ILogger<GetForecastsHandler> logger) : IQueryHandler<GetForecastsQuery, GetForecastsResult>
    {
        public async Task<GetForecastsResult> Handle(GetForecastsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetForecastsHandler.Handle called to retrieve all forecasts.");

            //get matches with Pagination
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await context.Forecasts.LongCountAsync(cancellationToken);

            var forecasts = (await context.Forecasts
                .ToForecastDtoListAsync(context, cancellationToken))
                .OrderByDescending(dto => dto.MatchDate)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            return new GetForecastsResult(
                new PaginatedResult<ForecastDto>(
                    pageIndex,
                    pageSize,
                    totalCount, forecasts));
        }
    }
}
