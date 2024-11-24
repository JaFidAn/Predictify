using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Prediction.Domain.Abstractions;

namespace Prediction.Infrastructure.Interceptors
{
    public class PublishDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
    {
        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            PublishDomainEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavedChanges(eventData, result);
        }

        public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            await PublishDomainEvents(eventData.Context);
            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        private async Task PublishDomainEvents(DbContext? context)
        {
            if (context is null) return;

            // Get all aggregates with domain events
            var aggregates = context.ChangeTracker
                .Entries<IAggregate>()
                .Where(entry => entry.Entity.DomainEvents.Any())
                .Select(entry => entry.Entity);

            // Extract and collect all domain events
            var domainEvents = aggregates
                .SelectMany(aggregate => aggregate.DomainEvents)
                .ToList();

            // Clear domain events from aggregates to prevent duplicate handling
            foreach (var aggregate in aggregates)
            {
                aggregate.ClearDomainEvents();
            }

            // Publish each domain event using MediatR
            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }
}
