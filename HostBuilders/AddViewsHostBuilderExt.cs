using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PingApp.ViewModels;
using PingApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.HostBuilders
{
    public static class AddViewsHostBuilderExt
    {
        public static IHostBuilder AddViews(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {
                services.AddSingleton<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
            });
        }
    }
}
