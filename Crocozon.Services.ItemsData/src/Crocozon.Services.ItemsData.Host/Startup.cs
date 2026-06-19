using Crocozon.Services.ItemsData.Application.Commands.CreateItem;
using Crocozon.Services.ItemsData.Application.Infrastructure;
using Crocozon.Services.ItemsData.Grpc.Servers;
using Crocozon.Services.ItemsData.Host.Registrations;
using Crocozon.Services.ItemsData.Persistence.Stores;

namespace Crocozon.Services.ItemsData.Host;

public sealed class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = configuration
            .GetSection("PostgresOptions")
            .GetValue<string>("ConnectionString")
            ?? throw new InvalidOperationException("Empty connection string");
        
        services.AddNpgsqlDataSource(connectionString);
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        services.AddEventStore();
        
        services.AddGrpc();
        services.AddGrpcReflection();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateItemsCommand).Assembly);
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