using Serilog;
using Serilog.Events;

namespace PetHouse.Web.Logging;

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
