using Microsoft.Extensions.Hosting;
using Serilog;


namespace PingApp.HostBuilders
{
    public static class AddSerilogHostBuilderExt
    {
        public static IHostBuilder AddSerilog(this IHostBuilder host)
        {
            return host.UseSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithThreadId();
            });
        }
    }
}
