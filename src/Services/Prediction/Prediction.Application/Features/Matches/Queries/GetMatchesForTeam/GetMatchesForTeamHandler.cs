namespace Prediction.Application.Features.Matches.Queries.GetMatchesForTeam
{
    public class GetMatchesForTeamHandler(IApplicationDbContext context, ILogger<GetMatchesForTeamHandler> logger) : IQueryHandler<GetMatchesForTeamQuery, GetMatchesForTeamResult>
    {
        public async Task<GetMatchesForTeamResult> Handle(GetMatchesForTeamQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetMatchesForTeamHandler.Handle called with {@Query}", query);

            //get matches by TeamId with Pagination
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await context.Matches
                .Where(m => m.Team1Id == TeamId.Of(query.TeamId) || m.Team2Id == TeamId.Of(query.TeamId))
                .LongCountAsync(cancellationToken);

            var matches = await context.Matches
                .Where(m => m.Team1Id == TeamId.Of(query.TeamId) || m.Team2Id == TeamId.Of(query.TeamId))
                .ToMatchDtoList(context.Teams, context.OutcomeTypes, context.MatchOutcomeTypes)
                .OrderByDescending(dto => dto.Date)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            //return result
            return new GetMatchesForTeamResult(
                new PaginatedResult<MatchDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    matches));
        }
    }
}
