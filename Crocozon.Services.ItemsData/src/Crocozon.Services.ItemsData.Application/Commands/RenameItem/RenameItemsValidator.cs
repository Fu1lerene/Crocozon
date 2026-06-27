using Crocozon.Library.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace Crocozon.Services.ItemsData.Application.Commands.RenameItem;

public class RenameItemsValidator : AbstractValidator<RenameItemsCommand>
{
    public RenameItemsValidator()
    {
        ValidateDuplicateIds();
        ValidateEmptyItemName();
    }

    private void ValidateDuplicateIds()
    {
        RuleFor(x => x.ItemNames).Custom((itemNames, ctx) =>
        {
            var duplicateIds = itemNames
                .GroupBy(i => i.ItemId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToArray();

            if (duplicateIds.Length == 0)
                return;

            ctx.AddFailure(new ValidationFailure(
                nameof(ItemName.ItemId),
                ExceptionMessages.DuplicateItemIdsRequest(duplicateIds))
            {
                CustomState = duplicateIds
            });
        });
    }

    private void ValidateEmptyItemName()
    {
        RuleFor(x => x.ItemNames).Custom((itemNames, ctx) =>
        {
            var invalidItemIds = itemNames
                .Where(x => string.IsNullOrWhiteSpace(x.Name))
                .Select(x => x.ItemId)
                .ToArray();
            
            if (invalidItemIds.Length == 0)
                return;

            ctx.AddFailure(new ValidationFailure(
                nameof(ItemName.Name),
                ExceptionMessages.EmptyItemName(invalidItemIds))
            {
                CustomState = invalidItemIds
            });
        });
    }
}