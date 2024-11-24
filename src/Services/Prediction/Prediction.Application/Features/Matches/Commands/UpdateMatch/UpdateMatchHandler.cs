namespace Prediction.Application.Features.Matches.Commands.UpdateMatch
{
    public class UpdateMatchHandler(IApplicationDbContext context, ILogger<UpdateMatchHandler> logger) : ICommandHandler<UpdateMatchCommand, UpdateMatchResult>
    {
        public async Task<UpdateMatchResult> Handle(UpdateMatchCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateMatchHandler.Handle called with {@Command}", command);

            // Fetch the match to update
            var matchId = MatchId.Of(command.Match.Id);
            var match = await context.Matches
                .FindAsync(new object[] { matchId }, cancellationToken);

            if (match is null)
            {
                logger.LogError("Match with ID {MatchId} not found.", command.Match.Id);
                throw new ObjectNotFoundException(command.Match.Id);
            }

            // Check if both teams exist
            await command.Match.ValidateTeamsExistAsync(context, cancellationToken);

            // Check for duplicate match
            await command.Match.CheckForDuplicateMatchAsync(context, match.Id, cancellationToken);

            // Update match with new values
            UpdateMatchWithNewValues(match, command.Match);

            // Save changes to the match
            context.Matches.Update(match);

            // Handle completed matches
            if (match.IsCompleted)
            {
                try
                {
                    // Determine and update outcomes
                    var outcomeIds = await match.DetermineOutcomesAsync(context, cancellationToken);
                    await match.UpdateMatchOutcomesAsync(context, cancellationToken);

                    // Evaluate forecast
                    await match.EvaluateForecastAsync(context, logger, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing outcomes or evaluations for MatchId: {MatchId}", matchId.Value);
                }
            }
            else
            {
                try
                {
                    // For incomplete matches, update forecasts
                    await match.CreateForecastForMatchAsync(context, logger, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error updating forecast for MatchId: {MatchId}", matchId.Value);
                }
            }

            // Save all changes to the database
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Match with ID {MatchId} updated successfully.", matchId.Value);

            return new UpdateMatchResult(true);
        }

        private void UpdateMatchWithNewValues(Match match, MatchDto matchDto)
        {
            match.Update(
                team1Id: TeamId.Of(matchDto.Team1Id),
                team2Id: TeamId.Of(matchDto.Team2Id),
                date: matchDto.Date,
                team1Goals: matchDto.Team1Goals,
                team2Goals: matchDto.Team2Goals
            );
        }
    }
}
