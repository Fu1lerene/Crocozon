using Crocozon.Library.ValueObjects;
using Crocozon.Library.ValueObjects.Helpers;
using Crocozon.Services.ItemsData.Application.Commands.Common;
using Crocozon.Services.ItemsData.Application.Commands.CreateItem;
using Crocozon.Services.ItemsData.Application.Commands.RenameItem;
using Crocozon.Services.ItemsData.Application.Infrastructure;
using Crocozon.Services.ItemsData.Application.Queries.GetItem;
using Crocozon.Services.ItemsData.Persistence.Models;
using Dapper;
using Npgsql;

namespace Crocozon.Services.ItemsData.Persistence.Stores;

public class ItemsDataStore(NpgsqlDataSource datasource) : IItemsDataStore
{
    public async Task<IReadOnlyCollection<ItemDataDto>> GetItemsByIds(GetItemsQuery query, CancellationToken cancellationToken)
    {
        await using var connection = await datasource.OpenConnectionAsync(cancellationToken);
        const string sql = 
            """
            select * from public.items
            where item_id = any(@itemIds);
            """;

        var items = await connection.QueryAsync<ItemDataDbDto>(sql, new { query.ItemIds });

        return [.. items.Select(x => new ItemDataDto(x.ItemId, x.Name, new Money(x.BasePrice, x.BasePriceCurrency)))];
    }
    
    public async Task RenameItems(RenameItemsCommand command, CancellationToken cancellationToken)
    {
        var itemNames = command.ItemNames;
        await using var connection = await datasource.OpenConnectionAsync(cancellationToken);
        const string sql = 
            """
            update public.items i
            set name = u.name
            from unnest(@itemIds, @names) as u(item_id, name)
            where i.item_id = u.item_id;
            """;
        
        await connection.ExecuteAsync(sql,
            new
            {
                itemIds = itemNames.Select(x => x.ItemId).ToArray(),
                names = itemNames.Select(x => x.Name).ToArray()
            });
    }
    
    public async Task CreateItems(CreateItemsCommand command, CancellationToken cancellationToken)
    {
        var items = command.Items;
        await using var connection = await datasource.OpenConnectionAsync(cancellationToken);
        const string sql = 
            """
            insert into public.items (item_id, name, base_price, base_price_currency)
            select * from unnest(@itemIds, @names, @basePrices, @basePricesCurrency);
            """;
        
        await connection.ExecuteAsync(sql,
            new
            {
                itemIds = items.Select(x => x.ItemId).ToArray(),
                names = items.Select(x => x.Name).ToArray(),
                basePrices = items.Select(x => x.BasePrice.Value).ToArray(),
                basePricesCurrency = items.Select(x => x.BasePrice.Currency.ToUpperString()).ToArray()
            });
    }
}