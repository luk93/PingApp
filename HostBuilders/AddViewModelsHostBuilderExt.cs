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
                services.AddSingleton<IPingAppViewModelFactory, PingAppViewModelFactory>();

                services.AddSingleton<DeviceListViewModel>();
                services.AddSingleton<ConfigViewModel>();
                services.AddSingleton<LoggsViewModel>();
                services.AddSingleton<StatusBarViewModel>();
                services.AddSingleton<MainViewModel>();

                services.AddSingleton<CreateViewModel<DeviceListViewModel>>(services => () => services.GetRequiredService<DeviceListViewModel>());
                services.AddSingleton<CreateViewModel<LoggsViewModel>>(services => () => services.GetRequiredService<LoggsViewModel>());
                services.AddSingleton<CreateViewModel<ConfigViewModel>>(services => () => services.GetRequiredService<ConfigViewModel>());

            });
        }
    }
}
