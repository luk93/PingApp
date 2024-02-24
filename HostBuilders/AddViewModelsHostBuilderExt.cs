﻿using Microsoft.Extensions.DependencyInjection;
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

                services.AddTransient<DeviceListViewModel>();
                services.AddTransient<ConfigViewModel>();
                services.AddTransient<LoggsViewModel>();
                services.AddTransient<StatusBarViewModel>();
                services.AddTransient<MainViewModel>();

                services.AddSingleton<CreateViewModel<DeviceListViewModel>>(services => () => services.GetRequiredService<DeviceListViewModel>());
                services.AddSingleton<CreateViewModel<LoggsViewModel>>(services => () => services.GetRequiredService<LoggsViewModel>());
                services.AddSingleton<CreateViewModel<ConfigViewModel>>(services => () => services.GetRequiredService<ConfigViewModel>());

            });
        }
    }
}
