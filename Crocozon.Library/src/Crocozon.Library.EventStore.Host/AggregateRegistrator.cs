using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.EventStore.Abstractions;
using Crocozon.Library.EventStore.Abstractions.Processing;
using Crocozon.Library.EventStore.Persistence;
using Crocozon.Library.EventStore.Postgres;
using Crocozon.Library.EventStore.Processing;
using Crocozon.Library.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace Crocozon.Library.EventStore.Host;

public static class AggregateRegistrator
{
    public static IServiceCollection AddEventStore(this IServiceCollection services, IEventSerializer eventSerializer)
    {
        services
            .AddSingleton<IEventStore, Postgres.EventStore>()
            .AddSingleton<IEventReader, EventReader>()
            .AddSingleton<IEventWriter, EventWriter>()
            .AddSingleton(eventSerializer);

        return services;
    }
    
    public static IServiceCollection AddEventRepository<TAggregate, TId>(this IServiceCollection services)
        where TAggregate : IAggregate<TId>
        where TId : IAggregateId
    {
        services
            .AddScoped<IAggregateProcessor<TAggregate, TId>, AggregateProcessor<TAggregate, TId>>()
            .AddScoped<IRepository<TAggregate, TId>, Repository<TAggregate, TId>>();
        
        return services;
    }

    public static IServiceCollection AddMetadata(this IServiceCollection services, string source)
    {
        services
            .AddScoped<RequestContext>()
            .AddScoped<IRequestContext>(sp => sp.GetRequiredService<RequestContext>())
            .AddScoped<IEventMetadataProvider>(sp =>
                new EventMetadataProvider(sp.GetRequiredService<IRequestContext>(), source));
        
        return services;
    }
}