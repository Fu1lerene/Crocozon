using Crocozon.Library.EventStore.Abstractions;
using Crocozon.Library.ValueObjects;
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
        // var newItems = new List<Item>();
        // var items = repository.GetAsync([.. command.Items.Select(x => new ItemDataItemId(x.ItemId))], cancellationToken);
        // await foreach (var (id, aggregate) in items)
        // {
        //     if (aggregate is not null)
        //         continue;
        //     
        //     newItems.Add(new Item(id, new ItemName("biba"), Money.Zero));
        // }
        // await repository.SaveAsync(newItems, cancellationToken);
        // await itemsDataStore.CreateItems(command, cancellationToken);
    }
}