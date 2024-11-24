namespace Prediction.Domain.Events
{
    public record MatchUpdatedEvent(MatchId MatchId) : IDomainEvent;
}
