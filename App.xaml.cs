using ControlzEx.Theming;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using PingApp.Db;
using PingApp.DbServices;
using PingApp.HostBuilders;
using PingApp.Models;
using PingApp.Stores;
using PingApp.ViewModels;
using Serilog;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PingApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost? _host;


        public App()
        {
            _host = CreateHostBuilder().Build();
        }
        public static IHostBuilder CreateHostBuilder(string[]? args = null)
        {
            return Host.CreateDefaultBuilder(args)
                       .AddConfiguration()
                       .AddSerilog()
                       .AddDbContext()
                       .AddStores()
                       .AddServices()
                       .AddViewModels()
                       .AddViews();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (_host != null)
            {
                _host.Start();
                var dbExist = false;
                AppDbContextFactory contextFactory = _host.Services.GetRequiredService<AppDbContextFactory>();
                using (var context = contextFactory.CreateDbContext())
                {
                    if(context.Database.CanConnect())
                    {
                        DeviceDbService? deviceDbService = _host.Services.GetRequiredService<DeviceDbService>();
                        DeviceListStore? deviceListStore = _host.Services.GetService<DeviceListStore>();
                        DeviceListViewModel? deviceListViewModel = _host.Services.GetService<DeviceListViewModel>();
                        dbExist = true;
                        var deviceList = (await deviceDbService?.GetAllWithPingResults())?.ToList() ?? [];
                        deviceListStore.Load(deviceList);
                    }
                    else
                        context.Database.EnsureCreated();
                }
                
                ThemeManager.Current.ChangeTheme(Application.Current, "Dark.Steel");
                Window window = _host.Services.GetRequiredService<MainWindow>();
                window.Show();
                if (dbExist) Log.Information($"Db already exists - Data loaded!");
                else Log.Information($"Db has been created!");

            }
            base.OnStartup(e);
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }
            base.OnExit(e);
        }
    }

}
