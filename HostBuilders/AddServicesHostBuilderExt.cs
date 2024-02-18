using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PingApp.DbServices;
using PingApp.Services;
using PingApp.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.HostBuilders
{
    public static class AddServicesHostBuilderExt
    {
        public static IHostBuilder AddServices(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {
                services.AddSingleton<DeviceRecordService>();
                services.AddSingleton<DeviceListService>();
                services.AddSingleton<DevicePingSender>();
            });
        }
    }
}
