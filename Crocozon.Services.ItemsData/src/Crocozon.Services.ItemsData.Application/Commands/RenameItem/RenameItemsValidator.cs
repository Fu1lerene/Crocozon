using Crocozon.Library.Exceptions;
using FluentValidation;

namespace Crocozon.Services.ItemsData.Application.Commands.RenameItem;

public class RenameItemsValidator : AbstractValidator<RenameItemsCommand>
{
    public RenameItemsValidator()
    {
        RuleFor(x => x.ItemNames)
            .Must(itemNames => itemNames.DistinctBy(x => x.ItemId).Count() == itemNames.Count)
            .WithMessage(ExceptionMessages.DuplicateItemIdsRequest);
    }
}