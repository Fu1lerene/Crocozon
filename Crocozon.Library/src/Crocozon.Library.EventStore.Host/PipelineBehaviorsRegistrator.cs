using Crocozon.Library.PipelineBehaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Crocozon.Library.EventStore.Host;

public static class PipelineBehaviorsRegistrator
{
    public static void AddCommonPipelineBehaviors(this MediatRServiceConfiguration cfg)
    {
        cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    }
}