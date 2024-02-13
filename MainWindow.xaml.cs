using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using OfficeOpenXml;
using PingApp.Extensions;
using PingApp.Models;
using PingApp.Services;
using PingApp.Tools;
using PingApp.ViewModel;
using Serilog;
using Serilog.Events;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Formats.Tar;
using System.IO;
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
        private readonly DevicePingSender _testPingSender;
        private readonly DeviceListService _deviceListService;
        private ObservableCollection<Device> _deviceList;
        private FileInfo? _xlsxFile;
        DeviceListViewModel DeviceListViewModel { get; set; } 
        public MainWindow()
        {
            InitializeComponent();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

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

            _deviceList = new ObservableCollection<Device>();
            _deviceListService = new DeviceListService(_deviceList);
            
            DeviceListViewModel = new(_deviceList);

            AutoResetEvent waiter = new(false);

            _testPingSender = new DevicePingSender(_deviceList, "################################", 3000, _logger, TbStatus);
            SubscribeDeviceChangeEvents();
            this.DataContext = DeviceListViewModel;

        }
        #region UI_EventHandlers
        private void _testPingSender_DeviceChanged(object? sender, EventArgs e)
        {
            LV_Devices.Items.Refresh();
        }

        private void B_TriggerAll_Click(object sender, RoutedEventArgs e)
        {
            if (DeviceListViewModel != null)
            {
                _testPingSender.SendPingToDeviceList();
            }
        }
        private async void B_GetDevicesFromExcel_ClickAsync(object sender, RoutedEventArgs e)
        {
            _xlsxFile = SelectXlsxFileAndTryToUse("Select excel file which contains Devices (Name,IP Address) (.xlsx)");
            if (_xlsxFile != null)
            {
                TbStatus.AddLine($"File ${_xlsxFile.FullName} selected!");
                await _deviceListService.UpdateDevicesFromExcelFile(_xlsxFile);
                DeviceListViewModel.UpdateDevices(_deviceList);
                SubscribeDeviceChangeEvents();
            }

        }
        #endregion
        #region UI_Functions
        public void DisableButtonAndChangeCursor(object sender)
        {
            Cursor = Cursors.Wait;
            Button button = (Button)sender;
            button.IsEnabled = false;
        }
        public void EnableButtonAndChangeCursor(object sender)
        {
            Cursor = Cursors.Arrow;
            Button button = (Button)sender;
            button.IsEnabled = true;
        }
        public FileInfo? SelectXlsxFileAndTryToUse(string title)
        {
            OpenFileDialog openFileDialog1 = new()
            {
                InitialDirectory = @"c:\Users\localadm\Desktop",
                Title = title,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "xlsx",
                Filter = "Excel file (*.xlsx)|*.xlsx",
                RestoreDirectory = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true,
            };
            if (openFileDialog1.ShowDialog() == true)
            {
                FileInfo xlsx = new(openFileDialog1.FileName);
                if (xlsx.Exists && !Tools.Tools.IsFileLocked(xlsx.FullName))
                {
                    return xlsx;
                }
                TbStatus.AddLine($"File not exist or in use!");
                return null;
            }
            TbStatus.AddLine($"File not selected!");
            return null;
        }
        public void SubscribeDeviceChangeEvents()
        {
            //Davice changed events subsription
            foreach (var device in DeviceListViewModel.Devices)
            {
                _testPingSender.DeviceChanged += device.HandleDeviceChanged;
            }
        }
        #endregion
    }
}