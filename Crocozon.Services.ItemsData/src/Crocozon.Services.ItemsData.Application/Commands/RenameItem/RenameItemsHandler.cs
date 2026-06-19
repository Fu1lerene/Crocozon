using Crocozon.Library.EventStore.Abstractions;
using Crocozon.Services.ItemsData.Application.Infrastructure;
using Crocozon.Services.ItemsData.Domain.Aggregates;
using Crocozon.Services.ItemsData.Domain.ValueObjects;
using MediatR;

namespace Crocozon.Services.ItemsData.Application.Commands.RenameItem;

public class RenameItemsHandler(IRepository<Item, ItemDataItemId> repository, IItemsDataStore itemsDataStore) : IRequestHandler<RenameItemsCommand>
{
    public async Task Handle(RenameItemsCommand command, CancellationToken cancellationToken)
    {
        foreach (var commandItemName in command.ItemNames)
        {
            var item = await repository.GetAsync(commandItemName.ItemId, cancellationToken);
            if (item is null)
                continue;
        
            item.RenameItem(new Domain.Events.ValueObjects.ItemName(commandItemName.Name));

            await repository.SaveAsync(item, cancellationToken);
        }
        await itemsDataStore.RenameItems(command, cancellationToken);
    }
}