namespace Prediction.Application.Features.Teams.Queries.GetTeamsWithMaxStreaks
{
    public class GetTeamsWithMaxStreaksHandler(IApplicationDbContext context, ILogger<GetTeamsWithMaxStreaksHandler> logger) : IQueryHandler<GetTeamsWithMaxStreaksQuery, GetTeamsWithMaxStreaksResult>
    {
        public async Task<GetTeamsWithMaxStreaksResult> Handle(GetTeamsWithMaxStreaksQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetTeamsWithMaxStreaksHandler.Handle called with {@Query}", query);

            // Get teams with Pagination
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            // Filter streak records for pagination
            var filteredTeamsQuery = context.Teams
                .TeamsWithMaxStreaksToDto(context)
                .OrderBy(t => t.TeamName);

            // Count total items before pagination
            var totalCount = await filteredTeamsQuery.CountAsync(cancellationToken);

            // Apply pagination
            var teams = await filteredTeamsQuery
                .ToListAsync(cancellationToken);

            //return result
            return new GetTeamsWithMaxStreaksResult(
                new PaginatedResult<TeamsWithMaxStreaksDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    teams));
        }
    }
}
