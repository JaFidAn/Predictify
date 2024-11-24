namespace Prediction.Application.Features.Teams.Queries.GetTeams
{
    public class GetTeamsHandler(IApplicationDbContext context, ILogger<GetTeamsHandler> logger) : IQueryHandler<GetTeamsQuery, GetTeamsResult>
    {
        public async Task<GetTeamsResult> Handle(GetTeamsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetTeamsHandler.Handle called with {@Query}", query);

            //get teams with Pagination
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await context.Teams.LongCountAsync(cancellationToken);

            var teams = (await context.Teams
                .ToTeamDtoListAsync(context.Leagues, context.Countries, cancellationToken))
                .OrderBy(dto => dto.League.Country.Name)
                .ThenBy(dto => dto.League.Name)
                .ThenBy(dto => dto.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToList();

            //return result
            return new GetTeamsResult(
                new PaginatedResult<TeamDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    teams));
        }
    }
}
