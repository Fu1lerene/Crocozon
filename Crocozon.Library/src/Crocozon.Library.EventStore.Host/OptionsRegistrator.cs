using Crocozon.Library.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crocozon.Library.EventStore.Host;

public static class OptionsRegistrator
{
    public static IServiceCollection AddCommonOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<PostgresOptions>()
            .Bind(configuration.GetSection("PostgresOptions"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services
            .AddOptions<LoggingBehaviorOptions>()
            .Bind(configuration.GetSection("LoggingBehavior"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        return services;
    }
}