using Crocozon.Library.EventStore.Host;
using Crocozon.Services.ItemsData.Application.Commands.CreateItem;
using FluentValidation;

namespace Crocozon.Services.ItemsData.Host.Registrators;

public static class PipelineBehaviorsRegistrator
{
    public static IServiceCollection AddPipelineBehaviors(this IServiceCollection services)
    {
        services.AddCommonPipelineBehaviors();
        
        services.AddValidatorsFromAssembly(typeof(CreateItemsCommand).Assembly);

        return services;
    }
}