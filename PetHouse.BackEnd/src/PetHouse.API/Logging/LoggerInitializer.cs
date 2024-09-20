namespace PetHouse.API.Logging;

using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Configuration;

public static class LoggerInitializer
{
    public static void ConfigureLogger(IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Seq(configuration.GetConnectionString("Seq") ?? throw new ArgumentNullException())
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .CreateLogger();
    }
}
