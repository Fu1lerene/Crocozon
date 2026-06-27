using Crocozon.Library.Exceptions;
using Crocozon.Services.ItemsData.Application.Commands.Common;
using FluentValidation;
using FluentValidation.Results;

namespace Crocozon.Services.ItemsData.Application.Commands.CreateItem;

public class CreateItemsValidator : AbstractValidator<CreateItemsCommand>
{
    public CreateItemsValidator()
    {
        ValidateDuplicateIds();
    }

    private void ValidateDuplicateIds()
    {
        RuleFor(x => x.Items).Custom((items, ctx) =>
        {
            var duplicateIds = items
                .GroupBy(i => i.ItemId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToArray();

            if (duplicateIds.Length == 0)
                return;

            ctx.AddFailure(new ValidationFailure(
                nameof(ItemDataDto.ItemId),
                ExceptionMessages.DuplicateItemIdsRequest(duplicateIds))
            {
                CustomState = duplicateIds
            });
        });
    }
}