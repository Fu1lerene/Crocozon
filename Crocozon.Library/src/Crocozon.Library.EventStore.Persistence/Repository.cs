using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.EventStore.Abstractions;

namespace Crocozon.Library.EventStore.Persistence;

public class Repository<TAggregate, TId>(IEventStore eventStore, IAggregateFactory<TAggregate, TId> factory) 
    : IRepository<TAggregate, TId>
    where TAggregate : IAggregate<TId>
    where TId : IAggregateId
{
    public Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken)
        => eventStore.SaveEventsAsync(aggregate.Id.Value, aggregate.Version, aggregate.UncommitedEvents, cancellationToken);

    public async Task<TAggregate?> GetAsync(TId id, CancellationToken cancellationToken)
    {
        var events = await eventStore.ReadEventsAsync(id.Value, cancellationToken);
        if (events.EventsData.Count == 0)
            return default;

        var domainEvents = events.EventsData.Select(x => x.Event).ToArray();

        return factory.Create(id, domainEvents);
    }
    
    // TODO: Сделать метод для работы с потоком агргегатов
}