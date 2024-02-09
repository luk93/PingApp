using Microsoft.Extensions.Logging;
using PingApp.Tools;
using PingApp.ViewModel;
using Serilog;
using Serilog.Events;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace PingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        private DeviceList _deviceList;
        private DevicePingSender _testPingSender;
        public MainWindow()
        {
            InitializeComponent();

            //Logger Configuration
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                LoggerConfiguration loggerConfiguration = new();
                loggerConfiguration.WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day)
                   .MinimumLevel.Information()
                   .MinimumLevel.Override("Logging: ", LogEventLevel.Debug);
                builder.AddSerilog(loggerConfiguration.CreateLogger());
            });
            _logger = _loggerFactory.CreateLogger("logger");
            _logger.LogInformation("Logging Started");

            _deviceList = new DeviceList();
            _deviceList.FillDeviceList();

            AutoResetEvent waiter = new AutoResetEvent(false);

            _testPingSender = new DevicePingSender(_deviceList, "################################", 3000, _logger);

            LV_Devices.ItemsSource = _deviceList.Devices;

        }
        private void B_TriggerAll_Click(object sender, RoutedEventArgs e)
        {
            if (_deviceList.Devices != null)
            {
                _testPingSender.SendPingToDeviceList();
            }
        }
        private void B_RefreshData_Click(object sender, RoutedEventArgs e)
        {
            LV_Devices.Items.Refresh();
        }
    }
}