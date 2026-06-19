
namespace Crocozon.Services.ItemsData.Host;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(options =>
                {
                    options.ConfigureEndpointDefaults(listenOptions =>
                    {
                        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
                    });
                });
                
                webBuilder.UseStartup<Startup>();
            }).Build();

        await host.RunAsync();
    }
}