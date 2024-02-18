using ControlzEx.Theming;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using PingApp.HostBuilders;
using Serilog;
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
                       .AddSerilog()
                       .AddDbContext()
                       .AddStores()
                       .AddServices()
                       .AddViewModels()
                       .AddViews();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (_host != null)
            {
                _host.Start();
                ThemeManager.Current.ChangeTheme(Application.Current, "Dark.Steel");
                Window window = _host.Services.GetRequiredService<MainWindow>();
                window.Show();
                Log.Information("Window opened!");
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
