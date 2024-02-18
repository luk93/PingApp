using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.HostBuilders
{
    public static class AddSerilogHostBuilderExt
    {
        public static IHostBuilder AddSerilog(this IHostBuilder host)
        {

            return host.UseSerilog((host, loggerConfiguration) =>
            {
                loggerConfiguration
                    .WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day)
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Logging: ", Serilog.Events.LogEventLevel.Debug);

            });
        }
    }
}
