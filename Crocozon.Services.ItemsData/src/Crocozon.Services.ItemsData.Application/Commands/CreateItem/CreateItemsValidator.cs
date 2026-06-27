using Crocozon.Library.Exceptions;
using FluentValidation;

namespace Crocozon.Services.ItemsData.Application.Commands.CreateItem;

public class CreateItemsValidator : AbstractValidator<CreateItemsCommand>
{
    public CreateItemsValidator()
    {
        RuleFor(x => x.Items)
            .Must(items => items.Distinct().Count() == items.Count)
            .WithMessage(ExceptionMessages.DuplicateItemIdsRequest);
    }
}