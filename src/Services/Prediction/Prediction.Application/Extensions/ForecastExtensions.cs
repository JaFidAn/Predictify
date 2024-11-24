namespace Prediction.Application.Extensions
{
    public static class ForecastExtensions
    {
        public static async Task CreateForecastForMatchAsync(this Match match, IApplicationDbContext context, ILogger logger, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting forecast calculation for MatchId: {MatchId}", match.Id.Value);

            // Skip forecast calculation for completed matches
            if (match.IsCompleted)
            {
                logger.LogWarning("MatchId: {MatchId} is completed. Skipping forecast creation.", match.Id.Value);
                return;
            }

            // Remove existing forecasts for this match
            var existingForecasts = await context.Forecasts
                .Where(f => f.MatchId == match.Id)
                .ToListAsync(cancellationToken);

            if (existingForecasts.Any())
            {
                context.Forecasts.RemoveRange(existingForecasts);
                logger.LogInformation("Removed {Count} existing forecasts for MatchId: {MatchId}.", existingForecasts.Count, match.Id.Value);
            }

            // Fetch streaks for both teams
            var team1Streaks = await match.Team1Id.GetTeamStreaksAsync(context, cancellationToken);
            var team2Streaks = await match.Team2Id.GetTeamStreaksAsync(context, cancellationToken);

            if (!team1Streaks.Any())
            {
                logger.LogWarning("No streak records found for Team1Id: {Team1Id}. Using default values.", match.Team1Id.Value);
            }

            if (!team2Streaks.Any())
            {
                logger.LogWarning("No streak records found for Team2Id: {Team2Id}. Using default values.", match.Team2Id.Value);
            }

            // Fetch all OutcomeTypes
            var outcomeTypes = await context.OutcomeTypes.ToListAsync(cancellationToken);

            if (!outcomeTypes.Any())
            {
                logger.LogWarning("No OutcomeTypes available in the system. Forecast cannot be created.");
                return;
            }

            // Calculate confidence for all outcome types
            var outcomeConfidences = outcomeTypes
                .Select(outcomeType =>
                {
                    var team1Streak = team1Streaks.GetValueOrDefault(outcomeType.Id, (0, 1));
                    var team2Streak = team2Streaks.GetValueOrDefault(outcomeType.Id, (0, 1));
                    var confidence = CalculateOutcomeConfidence(team1Streak, team2Streak);
                    return new { OutcomeTypeId = outcomeType.Id, Confidence = confidence };
                })
                .ToDictionary(x => x.OutcomeTypeId, x => x.Confidence);

            // Determine the outcome type with the highest confidence
            var (selectedOutcomeType, maxConfidence) = outcomeConfidences
                .OrderByDescending(c => c.Value)
                .FirstOrDefault();

            if (selectedOutcomeType == null)
            {
                logger.LogWarning("No valid outcome type selected for MatchId: {MatchId}.", match.Id.Value);
                return;
            }

            // Create and save forecast
            var forecast = Forecast.Create(
                ForecastId.Of(Guid.NewGuid()),
                MatchId.Of(match.Id.Value),
                OutcomeTypeId.Of(selectedOutcomeType.Value),
                maxConfidence
            );

            context.Forecasts.Add(forecast);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Forecast created for MatchId: {MatchId} with OutcomeTypeId: {OutcomeTypeId} and Confidence: {Confidence}",
                match.Id.Value, selectedOutcomeType.Value, maxConfidence);
        }

        private static decimal CalculateOutcomeConfidence((int CurrentStreak, int MaxStreak) team1Streak, (int CurrentStreak, int MaxStreak) team2Streak)
        {
            var team1Confidence = team1Streak.CurrentStreak / (decimal)Math.Max(team1Streak.MaxStreak, 1);
            var team2Confidence = team2Streak.CurrentStreak / (decimal)Math.Max(team2Streak.MaxStreak, 1);

            // Average the confidence of both teams
            return (team1Confidence + team2Confidence) / 2;
        }

        public static async Task EvaluateForecastAsync(this Match match, IApplicationDbContext context, ILogger logger, CancellationToken cancellationToken)
        {
            logger.LogInformation("Evaluating forecast for MatchId: {MatchId}", match.Id.Value);

            // Ensure the match is completed before evaluating
            if (!match.IsCompleted)
            {
                logger.LogWarning("MatchId: {MatchId} is not completed. Forecast evaluation skipped.", match.Id.Value);
                return;
            }

            // Fetch the actual outcome based on match results
            var actualOutcome = await match.DetermineActualOutcomeAsync(context, cancellationToken);

            // Fetch the forecast for this match
            var forecast = await context.Forecasts
                .FirstOrDefaultAsync(f => f.MatchId == match.Id, cancellationToken);

            if (forecast == null)
            {
                logger.LogWarning("No forecast found for MatchId: {MatchId}. Evaluation skipped.", match.Id.Value);
                return;
            }

            // Check if an evaluation already exists for this forecast
            var existingEvaluation = await context.ForecastEvaluations
                .FirstOrDefaultAsync(fe => fe.ForecastId == forecast.Id, cancellationToken);

            if (existingEvaluation != null)
            {
                logger.LogWarning("Forecast evaluation already exists for MatchId: {MatchId}. Skipping duplicate evaluation.", match.Id.Value);
                return;
            }

            // Evaluate the forecast
            var wasCorrect = forecast.OutcomeTypeId == actualOutcome;
            var forecastEvaluation = ForecastEvaluation.Create(
                ForecastEvaluationId.Of(Guid.NewGuid()),
                forecast.Id,
                actualOutcome,
                forecast.Confidence,
                wasCorrect
            );

            // Save the forecast evaluation
            await context.ForecastEvaluations.AddAsync(forecastEvaluation, cancellationToken);

            // Remove the forecast after evaluation
            context.Forecasts.Remove(forecast);

            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Forecast evaluation completed for MatchId: {MatchId}. WasCorrect: {WasCorrect}", match.Id.Value, wasCorrect);
        }

        public static async Task<OutcomeTypeId> DetermineActualOutcomeAsync(this Match match, IApplicationDbContext context, CancellationToken cancellationToken)
        {
            // Fetch outcome types
            var outcomeTypes = await context.OutcomeTypes.ToListAsync(cancellationToken);

            var outcomeTypeMap = outcomeTypes.ToDictionary(
                ot => ot.Name,
                ot => OutcomeTypeId.Of(ot.Id.Value)
            );

            // Determine the actual outcome
            if (match.Team1Goals > match.Team2Goals)
            {
                return outcomeTypeMap["Win"];
            }
            else if (match.Team1Goals < match.Team2Goals)
            {
                return outcomeTypeMap["Loss"];
            }
            else
            {
                return outcomeTypeMap["Draw"];
            }
        }

        public static async Task<List<ForecastDto>> ToForecastDtoListAsync(this IQueryable<Forecast> forecasts, IApplicationDbContext context, CancellationToken cancellationToken)
        {
            return await forecasts
                .Join(context.Matches,
                    forecast => forecast.MatchId,
                    match => match.Id,
                    (forecast, match) => new { forecast, match })
                .Join(context.OutcomeTypes,
                    fm => fm.forecast.OutcomeTypeId,
                    outcomeType => outcomeType.Id,
                    (fm, outcomeType) => new
                    {
                        fm.forecast,
                        fm.match,
                        outcomeType
                    })
                .Select(fmo => new ForecastDto
                {
                    ForecastId = fmo.forecast.Id.Value,
                    MatchId = fmo.match.Id.Value,
                    OutcomeTypeId = fmo.outcomeType.Id.Value,
                    OutcomeTypeName = fmo.outcomeType.Name,
                    Confidence = fmo.forecast.Confidence,
                    Team1Id = fmo.match.Team1Id.Value,
                    Team1Name = context.Teams.FirstOrDefault(t => t.Id == fmo.match.Team1Id)!.Name,
                    Team2Id = fmo.match.Team2Id.Value,
                    Team2Name = context.Teams.FirstOrDefault(t => t.Id == fmo.match.Team2Id)!.Name,
                    MatchDate = fmo.match.Date
                })
                .ToListAsync(cancellationToken);
        }
    }
}

