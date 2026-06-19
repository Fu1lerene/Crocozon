using Crocozon.Services.ItemsData.Application.Commands.Common;
using MediatR;

namespace Crocozon.Services.ItemsData.Application.Commands.CreateItem;

public record CreateItemsCommand(IReadOnlyCollection<ItemDataDto> Items) : IRequest;