using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PingApp.Models;
using PingApp.States.Navigators;
using PingApp.States.Ribbon;
using PingApp.Stores;
using PingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.HostBuilders
{
    public static class AddStoresHostBuilderExt
    {
        public static IHostBuilder AddStores(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {
                services.AddAutoMapper(typeof(App));
                services.AddSingleton<StatusStore>();
                services.AddSingleton<LoggsStore>();
                services.AddSingleton<IPingAppNavigator, PingAppNavigator>();
                services.AddSingleton<IPingAppRibbon, PingAppRibbon>();
                services.AddSingleton<DeviceListStore>();
            });
        }
    }
}
