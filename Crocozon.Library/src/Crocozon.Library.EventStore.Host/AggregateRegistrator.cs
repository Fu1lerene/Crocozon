using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.EventStore.Abstractions;
using Crocozon.Library.EventStore.Persistence;
using Crocozon.Library.EventStore.Postgres;
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
        return services.AddScoped<IRepository<TAggregate, TId>, Repository<TAggregate, TId>>();
    }
}