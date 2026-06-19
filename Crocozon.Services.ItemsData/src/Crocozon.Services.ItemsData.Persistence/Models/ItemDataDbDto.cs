namespace Crocozon.Services.ItemsData.Persistence.Models;

public record ItemDataDbDto(long ItemId, string Name, decimal BasePrice, string BasePriceCurrency);