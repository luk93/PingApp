using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PingApp.ViewModels;
using PingApp.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PingApp.ViewModels.Base.ViewModelBase;

namespace PingApp.HostBuilders
{
    public static class AddViewModelsHostBuilderExt
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {
                services.AddSingleton<IPingAppViewModelFactory<DeviceListViewModel>, PingAppViewModelFactory>();
                services.AddSingleton<IRootPingAppViewModelFactory, RootPingAppViewModelFactory>();

                services.AddSingleton<DeviceViewModel>();
                services.AddSingleton<DeviceListViewModel>();
                services.AddSingleton<LoggsViewModel>();
                services.AddSingleton<MainViewModel>();

                //services.AddSingleton<CreateViewModel<DeviceListViewModel>>(services => () => services.GetRequiredService<DeviceListViewModel>());
                //services.AddSingleton<CreateViewModel<LoggsViewModel>>(services => () => services.GetRequiredService<LoggsViewModel>());

            });
        }
    }
}
