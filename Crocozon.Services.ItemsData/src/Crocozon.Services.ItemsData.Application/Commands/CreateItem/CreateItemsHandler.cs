using Crocozon.Library.EventStore.Abstractions.Processing;
using Crocozon.Services.ItemsData.Application.Infrastructure;
using Crocozon.Services.ItemsData.Domain.Aggregates;
using Crocozon.Services.ItemsData.Domain.Events.ValueObjects;
using Crocozon.Services.ItemsData.Domain.ValueObjects;
using MediatR;

namespace Crocozon.Services.ItemsData.Application.Commands.CreateItem;

public class CreateItemsHandler(IAggregateProcessor<Item, ItemDataItemId> processor, IItemsDataStore itemsDataStore) : IRequestHandler<CreateItemsCommand>
{
    public async Task Handle(CreateItemsCommand command, CancellationToken cancellationToken)
    {
        await processor.For(command.Items, item => item.ItemId)
            .IfNotExists((id, cmd) => new Item(id, new ItemName(cmd.Name), cmd.BasePrice))
            .ExecuteAsync(cancellationToken);
        
        await itemsDataStore.CreateItems(command, cancellationToken); // Temporary, in future saving by kafka
    }
}