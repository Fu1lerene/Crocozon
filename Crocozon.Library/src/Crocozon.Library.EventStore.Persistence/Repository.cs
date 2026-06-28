using System.Runtime.CompilerServices;
using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.EventStore.Abstractions;
using Crocozon.Library.Metadata;

namespace Crocozon.Library.EventStore.Persistence;

public class Repository<TAggregate, TId>(
    IEventStore eventStore,
    IAggregateFactory<TAggregate, TId> factory,
    IEventMetadataProvider metadataProvider)
    : IRepository<TAggregate, TId>
    where TAggregate : IAggregate<TId>
    where TId : IAggregateId
{
    public async Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken)
    {
        var request = GetEventsWriteRequest(aggregate);
        await eventStore.SaveEventsAsync(request, cancellationToken);

        aggregate.ClearUncommittedEvents();
    }

    public async Task SaveAsync(IReadOnlyCollection<TAggregate> aggregates, CancellationToken cancellationToken)
    {
        var requests = aggregates.Select(GetEventsWriteRequest).ToArray();
        await eventStore.SaveEventsAsync(requests, cancellationToken);

        foreach (var aggregate in aggregates)
            aggregate.ClearUncommittedEvents();
    }

    public async Task<TAggregate?> GetAsync(TId id, CancellationToken cancellationToken)
    {
        var events = await eventStore.ReadEventsAsync(id.Value, cancellationToken);
        if (events.EventsData.Count == 0)
            return default;

        var domainEvents = events.EventsData.Select(x => x.Event).ToArray();

        return factory.Create(id, domainEvents);
    }

    public async IAsyncEnumerable<(TId, TAggregate?)> GetAsync(IReadOnlyCollection<TId> ids,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var idsByGuid = ids.Distinct().ToDictionary(id => id.Value);

        await foreach (var events in eventStore.ReadEventsAsync(idsByGuid.Keys, cancellationToken))
        {
            var aggregateId = idsByGuid[events.AggregateId];

            if (events.EventsData.Count == 0)
            {
                yield return (aggregateId, default);
                continue;
            }

            var domainEvents = events.EventsData.Select(x => x.Event).ToArray();

            yield return (aggregateId, factory.Create(aggregateId, domainEvents));
        }
    }

    private EventsWriteRequest GetEventsWriteRequest(TAggregate aggregate)
        => new(aggregate.Id.Value, aggregate.Version,
            [.. aggregate.UncommittedEvents.Select(@event => new EventsEnvelope(@event, metadataProvider.Create()))]);
}