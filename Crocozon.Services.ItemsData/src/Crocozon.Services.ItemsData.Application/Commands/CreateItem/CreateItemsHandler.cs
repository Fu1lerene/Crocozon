using Crocozon.Library.EventStore.Abstractions;
using Crocozon.Services.ItemsData.Application.Infrastructure;
using Crocozon.Services.ItemsData.Domain.Aggregates;
using Crocozon.Services.ItemsData.Domain.Events.ValueObjects;
using Crocozon.Services.ItemsData.Domain.ValueObjects;
using MediatR;

namespace Crocozon.Services.ItemsData.Application.Commands.CreateItem;

public class CreateItemsHandler(IRepository<Item, ItemDataItemId> repository, IItemsDataStore itemsDataStore) : IRequestHandler<CreateItemsCommand>
{
    public async Task Handle(CreateItemsCommand command, CancellationToken cancellationToken)
    {
        foreach (var i in command.Items)
        {
            var item = await repository.GetAsync(i.ItemId, cancellationToken);
            if (item is not null)
                continue;
        
            var newItem = new Item(i.ItemId, new ItemName(i.Name), i.BasePrice);

            await repository.SaveAsync(newItem, cancellationToken);
        }
        await itemsDataStore.CreateItems(command, cancellationToken);
    }
}