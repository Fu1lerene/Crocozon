using MediatR;

namespace Crocozon.Services.ItemsData.Application.Commands.RenameItem;

public record RenameItemsCommand(IReadOnlyCollection<ItemName> ItemNames) : IRequest;

public record ItemName(long ItemId, string Name);