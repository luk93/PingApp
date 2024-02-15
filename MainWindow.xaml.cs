using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
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
using TextFileExport.Db;
using PingApp.Db;
using System.Reflection;
using System.Diagnostics;
using PingApp.DbServices;
using System.Data;
using System.Windows.Interop;
using Dapper;

namespace PingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ILoggerFactory _loggerFactory;
        private ILogger _logger;
        private DevicePingSender _testPingSender;
        private DeviceListService _deviceListService;
        private List<Device> _deviceList;
        private FileInfo? _xlsxFile;
        private AppDbContextFactory _appDbContextFactory;
        public DeviceRecordService DeviceRecordService { get; set; }
        DeviceListViewModel DeviceListViewModel { get; set; } 

        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync();
        }
        private async void InitializeAsync()
        {
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

            //Device List
            _deviceList = new List<Device>();
            _deviceListService = new DeviceListService(_deviceList);
            DeviceListViewModel = new(_deviceList);

            //Internal DB
            var _connString = "Data Source=internalDb.db";
            void ConfigureDbContext(DbContextOptionsBuilder o) => o.LogTo(message => Debug.WriteLine(message))
                                                                   .UseLoggerFactory(_loggerFactory)
                                                                   .EnableSensitiveDataLogging()
                                                                   .UseSqlite(_connString);

            _appDbContextFactory = new AppDbContextFactory(ConfigureDbContext);
            DeviceRecordService = new DeviceRecordService(_appDbContextFactory);
            using (var context = _appDbContextFactory.CreateDbContext())
            {
                if (context.Database.CanConnect())
                {
                    TbStatus.AddLine($"{DateTime.Now} - Db already exists - Data loaded!");
                    _deviceList = (await DeviceRecordService.GetAll())?.ToList() ?? new List<Device>();
                    DeviceListViewModel.UpdateDevices(_deviceList);
                }
                else
                {
                    context.Database.EnsureCreated();
                    TbStatus.AddLine($"{DateTime.Now} - Db has been created!");
                }
            }
            
            //Pinger
            AutoResetEvent waiter = new(false);
            _testPingSender = new DevicePingSender(_deviceList, "################################", 3000, _logger, TbStatus, DeviceRecordService);
            //UI Binding
            SubscribeDeviceChangeEvents();
            this.DataContext = DeviceListViewModel;
        }
        #region UI_EventHandlers
        private void B_TriggerAll_Click(object sender, RoutedEventArgs e)
        {
            if (DeviceListViewModel != null)
            {
                _testPingSender.SendPingToDeviceList();
            }
        }
        private async void B_GetDevicesFromExcel_ClickAsync(object sender, RoutedEventArgs e)
        {
            DisableButtonAndChangeCursor(sender);
            _xlsxFile = SelectXlsxFileAndTryToUse("Select excel file which contains Devices (Name,IP Address) (.xlsx)");
            if (_xlsxFile == null)
            {
                EnableButtonAndChangeCursor(sender);
                return;
            }
            TbStatus.AddLine($"File ${_xlsxFile.FullName} selected!");
            _deviceList = await _deviceListService.UpdateDevicesFromExcelFile(_xlsxFile);
            DeviceListViewModel.UpdateDevices(_deviceList);
            SubscribeDeviceChangeEvents();
            try
            {
                await DeviceRecordService.DeleteAll();
                foreach (var device in _deviceList)
                {
                    await DeviceRecordService.Create(device);
                }
            }
            catch (Exception ex) 
            {
                var msg = $"Error message: {ex.Message}, Stack: {ex.StackTrace}";
                _logger.LogError(msg);
                TbStatus.AddLine(msg);
            }
            EnableButtonAndChangeCursor(sender);

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