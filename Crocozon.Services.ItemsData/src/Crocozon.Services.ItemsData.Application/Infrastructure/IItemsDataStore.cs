using Crocozon.Services.ItemsData.Application.Commands.Common;
using Crocozon.Services.ItemsData.Application.Commands.CreateItem;
using Crocozon.Services.ItemsData.Application.Commands.RenameItem;
using Crocozon.Services.ItemsData.Application.Queries.GetItem;

namespace Crocozon.Services.ItemsData.Application.Infrastructure;

public interface IItemsDataStore
{
    Task<IReadOnlyCollection<ItemDataDto>> GetItemsByIds(GetItemsQuery query, CancellationToken cancellationToken);
    Task RenameItems(RenameItemsCommand command, CancellationToken cancellationToken);
    Task CreateItems(CreateItemsCommand command, CancellationToken cancellationToken);
}