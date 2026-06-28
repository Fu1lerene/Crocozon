using Crocozon.Library.EventStore.Host;
using Crocozon.Library.Options;
using Crocozon.Services.ItemsData.Application.Commands.CreateItem;
using Crocozon.Services.ItemsData.Application.Infrastructure;
using Crocozon.Services.ItemsData.Grpc.Servers;
using Crocozon.Services.ItemsData.Host.Registrators;
using Crocozon.Services.ItemsData.Persistence.Stores;
using FluentValidation;

namespace Crocozon.Services.ItemsData.Host;

public sealed class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCommonOptions(configuration);

        var connectionString = configuration
            .GetSection("PostgresOptions")
            .Get<PostgresOptions>()!
            .ConnectionString;
        
        services.AddNpgsqlDataSource(connectionString);
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        services.AddEventStore();
        
        services.AddGrpc(options =>
        {
            options.AddCommonInterceptors();
        });
        services.AddGrpcReflection();

        services.AddValidatorsFromAssembly(typeof(CreateItemsCommand).Assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateItemsCommand).Assembly);
            cfg.AddCommonPipelineBehaviors();
        });
        
        services.AddSingleton<IItemsDataStore, ItemsDataStore>();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<ItemsProvider>(); 
            
            endpoints.MapGrpcReflectionService();
        });
    }
}