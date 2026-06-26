using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.EventStore.Abstractions;
using Crocozon.Services.ItemsData.Domain.Aggregates;
using Crocozon.Services.ItemsData.Domain.ValueObjects;

namespace Crocozon.Services.ItemsData.Domain;

public class ItemDataAggregateFactory : IAggregateFactory<Item, ItemDataItemId>
{
    public Item Create(ItemDataItemId id, IReadOnlyCollection<IDomainEvent> events) => new(id, events);
}