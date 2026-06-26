using Crocozon.Library.EventStore.Abstractions;
using Crocozon.Library.EventStore.Host;
using Crocozon.Services.ItemsData.Domain;
using Crocozon.Services.ItemsData.Domain.Aggregates;
using Crocozon.Services.ItemsData.Domain.Events.Protobuf;
using Crocozon.Services.ItemsData.Domain.ValueObjects;


namespace Crocozon.Services.ItemsData.Host.Registrations;

public static class EventStoreRegistration
{
    public static IServiceCollection AddEventStore(this IServiceCollection services)
    {
        services
            .AddEventStore(DomainEventSerializer.Instance)
            .AddSingleton<IAggregateFactory<Item, ItemDataItemId>, ItemDataAggregateFactory>()
            .AddEventRepository<Item, ItemDataItemId>();

        return services;
    }
}