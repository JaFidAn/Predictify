using MediatR;
using Prediction.Domain.Events;

namespace Prediction.Application.Features.Matches.EventHandlers.Domain
{
    public class MatchDeletedEventHandler(IApplicationDbContext context, ILogger<MatchDeletedEventHandler> logger) : INotificationHandler<MatchDeletedEvent>
    {
        public async Task Handle(MatchDeletedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("MatchDeletedEventHandler triggered for MatchId: {MatchId}", notification.MatchId);

            // Fetch the match details (Team1Id, Team2Id, and Date) from the database
            var match = await context.Matches
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == notification.MatchId, cancellationToken);

            if (match == null)
            {
                logger.LogWarning("Match with ID {MatchId} not found. Unable to process TeamHistories removal.", notification.MatchId);
                return;
            }

            // Fetch all TeamHistories related to this match
            var teamHistoriesToRemove = await context.TeamHistories
                .Where(th =>
                    (th.TeamId == match.Team1Id && th.OpponentId == match.Team2Id && th.Date == match.Date) ||
                    (th.TeamId == match.Team2Id && th.OpponentId == match.Team1Id && th.Date == match.Date))
                .ToListAsync(cancellationToken);

            if (teamHistoriesToRemove.Any())
            {
                logger.LogInformation("Removing {Count} TeamHistories related to MatchId {MatchId}.", teamHistoriesToRemove.Count, notification.MatchId);
                context.TeamHistories.RemoveRange(teamHistoriesToRemove);
            }
            else
            {
                logger.LogInformation("No TeamHistories found for MatchId {MatchId}.", notification.MatchId);
            }

            // Save changes
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("MatchDeletedEventHandler successfully processed for MatchId {MatchId}.", notification.MatchId);
        }
    }
}
