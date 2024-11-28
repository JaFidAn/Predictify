namespace Prediction.Application.Features.ForecastEvaluations.Queries.GetForecastEvaluations
{
    public class GetForecastEvaluationsHandler(
        IApplicationDbContext context,
        ILogger<GetForecastEvaluationsHandler> logger
    ) : IQueryHandler<GetForecastEvaluationsQuery, GetForecastEvaluationsResult>
    {
        public async Task<GetForecastEvaluationsResult> Handle(GetForecastEvaluationsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetForecastEvaluationsHandler.Handle called with PageIndex: {PageIndex}, PageSize: {PageSize}.",
                query.PaginationRequest.PageIndex, query.PaginationRequest.PageSize);

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            // Fetch total count of evaluations for pagination
            var totalCount = await context.ForecastEvaluations.LongCountAsync(cancellationToken);

            // Fetch paginated ForecastEvaluationDto list
            var paginatedForecastEvaluations = await context.ForecastEvaluations
                .OrderByDescending(evaluation => evaluation.Id) // Adjust ordering if necessary
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToForecastEvaluationDtoListAsync(context, logger, cancellationToken);

            return new GetForecastEvaluationsResult(
                new PaginatedResult<ForecastEvaluationDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    paginatedForecastEvaluations
                )
            );
        }
    }
}
