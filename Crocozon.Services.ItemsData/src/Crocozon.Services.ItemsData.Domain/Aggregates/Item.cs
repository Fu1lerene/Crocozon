
using Crocozon.Library.Domain;
using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.ValueObjects;
using Crocozon.Services.ItemsData.Domain.ValueObjects;
using Crocozon.Services.ItemsData.Domain.Events;
using Crocozon.Services.ItemsData.Domain.Events.ValueObjects;

namespace Crocozon.Services.ItemsData.Domain.Aggregates;

public class Item : Aggregate<ItemDataItemId>
{
    public ItemName Name { get; private set; }
    public Money BasePrice { get; private set; }

    public Item(ItemDataItemId id, IReadOnlyCollection<IDomainEvent> events) : base(id)
    {
        ApplyCommitedEvents(events);
    }
    
    public Item(ItemDataItemId id, ItemName name, Money basePrice) : base(id)
    {
        ApplyEvent(new ItemCreated(id, name, basePrice));
    }

    public void RenameItem(ItemName name)
    {
        if (Name.Equals(name))
            return;
        
        ApplyEvent(new ItemNameChanged(Id, name));
    }
    
    protected override void OnApplyEvent(IDomainEvent @event)
        => When((dynamic)@event);

    private void When(ItemCreated e)
    {
        BasePrice = e.BasePrice;
        Name = e.Name;
    }
    
    private void When(ItemNameChanged e)
    {
        Name = e.Name;
    }
}