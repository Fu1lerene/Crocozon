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
            .Validate(x => !string.IsNullOrWhiteSpace(x.ConnectionString), "PostgresOptions.ConnectionString is required.")
            .ValidateOnStart();
        
        services
            .AddOptions<LoggingBehaviorOptions>()
            .Bind(configuration.GetSection("LoggingBehavior"))
            .Validate(x => x.SlowRequestThresholdMs > 0, "LoggingBehaviorOptions.SlowRequestThresholdMs must be positive.")
            .ValidateOnStart();
        
        return services;
    }
}