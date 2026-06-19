using Crocozon.Library.Extensions.Protobuf;
using Crocozon.Services.ItemsData.Application.Commands.Common;
using Crocozon.Services.ItemsData.Application.Commands.CreateItem;
using Crocozon.Services.ItemsData.Application.Commands.RenameItem;
using Crocozon.Services.ItemsData.Application.Queries.GetItem;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;

namespace Crocozon.Services.ItemsData.Grpc.Servers;

public class ItemsProvider(ISender sender) : Grpc.ItemsProvider.ItemsProviderBase
{
    public override async Task<Empty> CreateItems(CreateItemsRequest request, ServerCallContext context)
    {
        var items = request.Items
            .Select(x => new ItemDataDto(x.ItemId, x.Name, x.BasePrice.ToDomain()))
            .ToArray();
        var command = new CreateItemsCommand(items);
        
        await sender.Send(command, context.CancellationToken);
        
        return new Empty();
    }

    public override async Task<Empty> RenameItems(RenameItemsRequest request, ServerCallContext context)
    {
        var itemNames = request.ItemNames
            .Select(x => new ItemName(x.ItemId, x.Name))
            .ToArray();

        var command = new RenameItemsCommand(itemNames);
        await sender.Send(command, context.CancellationToken);
        
        return new Empty();
    }

    public override async Task<GetItemsResponse> GetItems(GetItemsRequest request, ServerCallContext context)
    {
        var query = new GetItemsQuery(request.ItemIds);
        var items = await sender.Send(query, context.CancellationToken);

        return new GetItemsResponse
        {
            Items =
            {
                items.Select(x => new ItemDataProto
                {
                    ItemId = x.ItemId,
                    Name = x.Name,
                    BasePrice = x.BasePrice.ToProto()
                })
            }
        };
    }
}