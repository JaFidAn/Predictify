namespace Prediction.Domain.Events
{
    public record ForecastEvaluationCreatedEvent(MatchId MatchId) : IDomainEvent;
}
