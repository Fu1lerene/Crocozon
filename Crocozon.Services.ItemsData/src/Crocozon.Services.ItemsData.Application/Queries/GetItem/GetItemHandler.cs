using Crocozon.Services.ItemsData.Application.Commands.Common;
using Crocozon.Services.ItemsData.Application.Infrastructure;
using MediatR;

namespace Crocozon.Services.ItemsData.Application.Queries.GetItem;

public class GetItemHandler(IItemsDataStore itemsDataStore) : IRequestHandler<GetItemsQuery, IReadOnlyCollection<ItemDataDto>>
{
    public async Task<IReadOnlyCollection<ItemDataDto>> Handle(GetItemsQuery query, CancellationToken cancellationToken)
    {
        return await itemsDataStore.GetItemsByIds(query, cancellationToken);
    }
}