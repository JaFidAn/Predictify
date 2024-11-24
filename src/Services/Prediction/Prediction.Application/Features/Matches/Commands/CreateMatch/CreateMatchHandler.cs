namespace Prediction.Application.Features.Matches.Commands.CreateMatch
{
    public class CreateMatchHandler(IApplicationDbContext context, ILogger<CreateMatchHandler> logger) : ICommandHandler<CreateMatchCommand, CreateMatchResult>
    {
        public async Task<CreateMatchResult> Handle(CreateMatchCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("CreateMatchHandler.Handle called with {@Command}", command);

            // Check if both teams exist
            await command.Match.ValidateTeamsExistAsync(context, cancellationToken);

            // Check for duplicate match
            await command.Match.CheckForDuplicateMatchAsync(context, null, cancellationToken);

            // Create Match entity
            var match = CreateNewMatch(command.Match);

            // Add Match to database
            await context.Matches.AddAsync(match, cancellationToken);

            // If the match is completed, determine and save outcomes
            if (match.IsCompleted)
            {
                try
                {
                    // Determine outcomes for completed match
                    var outcomeIds = await match.DetermineOutcomesAsync(context, cancellationToken);
                    await match.AddMatchOutcomeTypesAsync(outcomeIds, context, cancellationToken);

                    // Evaluate forecast (if any exists)
                    await match.EvaluateForecastAsync(context, logger, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing completed match with MatchId: {MatchId}", match.Id.Value);
                }
            }
            else
            {
                // If the match is not completed, calculate forecasts
                try
                {
                    await match.CreateForecastForMatchAsync(context, logger, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error creating forecast for MatchId: {MatchId}", match.Id.Value);
                }
            }

            // Save changes to the database
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Match with ID {MatchId} created successfully.", match.Id.Value);

            return new CreateMatchResult(match.Id.Value);
        }

        private Match CreateNewMatch(MatchDto matchDto)
        {
            return Match.Create(
                id: MatchId.Of(Guid.NewGuid()),
                team1Id: TeamId.Of(matchDto.Team1Id),
                team2Id: TeamId.Of(matchDto.Team2Id),
                date: matchDto.Date,
                team1Goals: matchDto.Team1Goals,
                team2Goals: matchDto.Team2Goals
            );
        }
    }
}
