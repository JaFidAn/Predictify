namespace Prediction.Domain.Events
{
    public record MatchDeletedEvent(MatchId MatchId) : IDomainEvent;
}
