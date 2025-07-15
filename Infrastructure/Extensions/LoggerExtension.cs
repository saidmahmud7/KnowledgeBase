using Microsoft.Extensions.Hosting;
using Serilog;

namespace Infrastructure.Extensions;

public static class LoggerExtension
{
    public static void AddSerilogLogger(this IHostBuilder hostBuilder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Loggers/logs.txt", rollingInterval: RollingInterval.Month)
            .Filter.ByExcluding(logEvent =>
            {
                if (!logEvent.Properties.TryGetValue("SourceContext", out var sourceContext)) return false;
                var sourceContextValue = sourceContext.ToString();
                return sourceContextValue.Contains("Microsoft.EntityFrameworkCore.Database.Command");
            })
            .CreateLogger();
    }
}