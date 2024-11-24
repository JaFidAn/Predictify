namespace Prediction.Domain.Events
{
    public record MatchCreatedEvent(MatchId MatchId) : IDomainEvent;

}
