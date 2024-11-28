namespace Prediction.Application.Extensions
{
    public static class ForecastEvaluationExtensions
    {
        public static async Task<List<ForecastEvaluationDto>> ToForecastEvaluationDtoListAsync(this IQueryable<ForecastEvaluation> forecastEvaluations, IApplicationDbContext context, ILogger logger, CancellationToken cancellationToken)
        {
            logger.LogInformation("Converting ForecastEvaluations to ForecastEvaluationDto list.");

            // Preload necessary data to avoid repeated queries
            var teams = await context.Teams
                .ToDictionaryAsync(t => t.Id, t => t.Name, cancellationToken);

            var outcomeTypes = await context.OutcomeTypes
                .ToDictionaryAsync(ot => ot.Id, ot => new { ot.Name, ot.Description }, cancellationToken);

            // Create a list for the results
            var results = new List<ForecastEvaluationDto>();

            // Fetch and iterate through the evaluations with their associated matches
            var evaluations = await forecastEvaluations
                .Join(context.Matches,
                      evaluation => evaluation.MatchId,
                      match => match.Id,
                      (evaluation, match) => new { evaluation, match })
                .ToListAsync(cancellationToken);

            foreach (var em in evaluations)
            {
                // Use DetermineActualOutcomesAsync for dynamic calculation of ActualOutcomes
                var actualOutcomeIds = await context.DetermineActualOutcomesAsync(em.match, cancellationToken);

                var actualOutcomes = actualOutcomeIds.Select(outcomeId =>
                {
                    if (outcomeTypes.TryGetValue(outcomeId, out var outcomeType))
                    {
                        return new OutcomeTypeDto
                        {
                            Id = outcomeId.Value,
                            Name = outcomeType.Name,
                            Description = outcomeType.Description // Include description
                        };
                    }

                    return new OutcomeTypeDto
                    {
                        Id = outcomeId.Value,
                        Name = "Unknown",
                        Description = "Unknown"
                    };
                }).ToList();

                // Create and add the DTO
                results.Add(new ForecastEvaluationDto
                {
                    Id = em.evaluation.Id.Value,
                    Date = em.match.Date,
                    Team1Name = teams.TryGetValue(em.match.Team1Id, out var team1Name) ? team1Name : "Unknown",
                    Team2Name = teams.TryGetValue(em.match.Team2Id, out var team2Name) ? team2Name : "Unknown",
                    MatchDate = em.match.Date,
                    ForecastOutcome = em.evaluation.ForecastOutcomeId.Value,
                    ForecastOutcomeName = outcomeTypes.TryGetValue(em.evaluation.ForecastOutcomeId, out var forecastOutcome)
                        ? forecastOutcome.Name
                        : "Unknown",
                    ActualOutcomes = actualOutcomes,
                    Confidence = em.evaluation.ConfidenceScore,
                    WasCorrect = em.evaluation.WasCorrect
                });
            }

            logger.LogInformation("Conversion to ForecastEvaluationDto list completed successfully.");
            return results;
        }
    }
}
