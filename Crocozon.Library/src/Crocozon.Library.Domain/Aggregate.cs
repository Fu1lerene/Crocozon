using Crocozon.Library.Domain.Abstractions;

namespace Crocozon.Library.Domain;

public abstract class Aggregate<TId> : IAggregate<TId>
    where TId : IAggregateId
{
    private readonly List<IDomainEvent> _uncommittedEvents = [];

    public TId Id { get; }
    public long Version { get; private set; } = -1;
    public IReadOnlyCollection<IDomainEvent> UncommittedEvents => _uncommittedEvents.AsReadOnly();

    protected Aggregate(TId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }

    public void ClearUncommittedEvents() => _uncommittedEvents.Clear();

    protected abstract void OnApplyEvent(IDomainEvent @event);

    protected void ApplyEvent(IDomainEvent @event)
    {
        Apply(@event);
        _uncommittedEvents.Add(@event);
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