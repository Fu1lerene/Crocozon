using Crocozon.Library.EventStore.Abstractions.Processing;
using Crocozon.Services.ItemsData.Application.Infrastructure;
using Crocozon.Services.ItemsData.Domain.Aggregates;
using Crocozon.Services.ItemsData.Domain.ValueObjects;
using MediatR;
using ItemNameValue = Crocozon.Services.ItemsData.Domain.Events.ValueObjects.ItemName;

namespace Crocozon.Services.ItemsData.Application.Commands.RenameItem;

public class RenameItemsHandler(IAggregateProcessor<Item, ItemDataItemId> processor, IItemsDataStore itemsDataStore) : IRequestHandler<RenameItemsCommand>
{
    public async Task Handle(RenameItemsCommand command, CancellationToken cancellationToken)
    {
        await processor.For(command.ItemNames, itemName => itemName.ItemId)
            .ThrowIfNotExists()
            .IfExists((item, cmd) => item.RenameItem(new ItemNameValue(cmd.Name)))
            .ExecuteAsync(cancellationToken);
        
        await itemsDataStore.RenameItems(command, cancellationToken); // Temporary, in future saving by kafka
    }
}