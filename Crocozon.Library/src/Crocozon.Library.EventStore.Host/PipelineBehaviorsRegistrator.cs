using Crocozon.Library.PipelineBehaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Crocozon.Library.EventStore.Host;

public static class PipelineBehaviorsRegistrator
{
    public static IServiceCollection AddCommonPipelineBehaviors(this IServiceCollection services)
    {
        services
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return services;
    }
}