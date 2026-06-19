using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.ValueObjects;
using Crocozon.Services.ItemsData.Domain.Events.ValueObjects;

namespace Crocozon.Services.ItemsData.Domain.Events;

public record ItemNameChanged(ItemId ItemId, ItemName Name) : IDomainEvent;