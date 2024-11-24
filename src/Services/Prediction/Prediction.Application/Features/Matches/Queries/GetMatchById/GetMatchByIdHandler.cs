namespace Prediction.Application.Features.Matches.Queries.GetMatchById
{
    public class GetMatchByIdHandler(IApplicationDbContext context, ILogger<GetMatchByIdHandler> logger) : IQueryHandler<GetMatchByIdQuery, GetMatchByIdResult>
    {
        public async Task<GetMatchByIdResult> Handle(GetMatchByIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetMatchByIdHandler.Handle called with {@Query}", query);

            //get match by id using context
            var match = await context.Matches
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == MatchId.Of(query.Id), cancellationToken);

            if (match is null)
            {
                throw new ObjectNotFoundException(query.Id);
            }

            // Fetch related data required for ToMatchDto
            var teams = context.Teams.AsNoTracking();
            var outcomeTypes = context.OutcomeTypes.AsNoTracking();
            var matchOutcomeTypes = context.MatchOutcomeTypes.AsNoTracking();

            // Return the result using ToMatchDto
            return new GetMatchByIdResult(match.ToMatchDto(teams, outcomeTypes, matchOutcomeTypes));
        }
    }
}
