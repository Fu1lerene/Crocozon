using Crocozon.Library.ValueObjects;

namespace Crocozon.Services.ItemsData.Application.Commands.Common;

public record ItemDataDto(long ItemId, string Name, Money BasePrice);