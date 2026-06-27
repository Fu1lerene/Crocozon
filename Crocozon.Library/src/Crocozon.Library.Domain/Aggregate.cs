using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.Metadata;

namespace Crocozon.Library.Domain;

public abstract class Aggregate<TId> : IAggregate<TId>
    where TId : IAggregateId
{
    private readonly List<EventsEnvelope> _uncommittedEvents = [];
    private EventMetadata _eventMetadata = new();

    public TId Id { get; }
    public long Version { get; private set; } = -1;
    public IReadOnlyCollection<EventsEnvelope> UncommittedEvents => _uncommittedEvents.AsReadOnly();

    protected Aggregate(TId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }

    public void ClearUncommittedEvents() => _uncommittedEvents.Clear();

    public void SetEventsMetadata(EventMetadata eventMetadata)
    {
        ArgumentNullException.ThrowIfNull(eventMetadata);
        
        _eventMetadata = eventMetadata;
    }
    
    protected abstract void OnApplyEvent(IDomainEvent @event);

    protected void ApplyEvent(IDomainEvent @event)
    {
        Apply(@event);
        _uncommittedEvents.Add(new EventsEnvelope(@event, _eventMetadata));
    }
    
    protected void ApplyCommitedEvents(IReadOnlyCollection<IDomainEvent> events)
    {
        foreach (var @event in events)
        {
            Apply(@event);
        }
    }

    private void Apply(IDomainEvent @event)
    {
        OnApplyEvent(@event);
        Version++;
    }
}