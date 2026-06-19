using Crocozon.Services.ItemsData.Application.Commands.Common;
using MediatR;

namespace Crocozon.Services.ItemsData.Application.Queries.GetItem;

public record GetItemsQuery(IReadOnlyCollection<long> ItemIds) : IRequest<IReadOnlyCollection<ItemDataDto>>;