using System.Windows;

namespace PingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //public partial class MainWindow : Window
    //{
    //    private ILoggerFactory _loggerFactory;
    //    private ILogger _logger;
    //    private DevicePingSender _testPingSender;
    //    private DeviceListService _deviceListService;
    //    private List<Device> _deviceList;
    //    private FileInfo? _xlsxFile;
    //    private AppDbContextFactory _appDbContextFactory;
    //    public DeviceRecordService DeviceRecordService { get; set; }
    //    DeviceListViewModel DeviceListViewModel { get; set; } 

    //    public MainWindow()
    //    {
    //        InitializeComponent();
    //        InitializeAsync();
    //    }
    //    private async void InitializeAsync()
    //    {
    //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    //        //Logger Configuration
    //        _loggerFactory = LoggerFactory.Create(builder =>
    //        {
    //            LoggerConfiguration loggerConfiguration = new();
    //            loggerConfiguration.WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day)
    //               .MinimumLevel.Information()
    //               .MinimumLevel.Override("Logging: ", LogEventLevel.Debug);
    //            builder.AddSerilog(loggerConfiguration.CreateLogger());
    //        });
    //        _logger = _loggerFactory.CreateLogger("logger");
    //        _logger.LogInformation("Logging Started");

    //        //Device List
    //        _deviceList = new List<Device>();
    //        _deviceListService = new DeviceListService(_deviceList);
    //        //VieModel
    //        _triggerAllCommand = new TriggerAllCommand();
    //        _getDevicesFromExcelCommand = new GetDevicesFromExcelCommand(_deviceList);
    //        DeviceListViewModel = new(_deviceList, _triggerAllCommand, _getDevicesFromExcelCommand);

    //        //Internal DB
    //        var _connString = "Data Source=internalDb.db";
    //        void ConfigureDbContext(DbContextOptionsBuilder o) => o.LogTo(message => Debug.WriteLine(message))
    //                                                               .UseLoggerFactory(_loggerFactory)
    //                                                               .EnableSensitiveDataLogging()
    //                                                               .UseSqlite(_connString);

    //        _appDbContextFactory = new AppDbContextFactory(ConfigureDbContext);
    //        DeviceRecordService = new DeviceRecordService(_appDbContextFactory);
    //        using (var context = _appDbContextFactory.CreateDbContext())
    //        {
    //            if (context.Database.CanConnect())
    //            {
    //                TbStatus.AddLine($"{DateTime.Now} - Db already exists - Data loaded!");
    //                _deviceList = (await DeviceRecordService.GetAll())?.ToList() ?? new List<Device>();
    //                DeviceListViewModel.UpdateDevices(_deviceList);
    //            }
    //            else
    //            {
    //                context.Database.EnsureCreated();
    //                TbStatus.AddLine($"{DateTime.Now} - Db has been created!");
    //            }
    //        }

    //        //Pinger
    //        AutoResetEvent waiter = new(false);
    //        _testPingSender = new DevicePingSender(_deviceList, "################################", 3000, _logger, TbStatus, DeviceRecordService);
    //        //UI Binding
    //        SubscribeDeviceChangeEvents();
    //        this.DataContext = DeviceListViewModel;
    //    }
    //    #region UI_EventHandlers
    //    public void SubscribeDeviceChangeEvents()
    //    {
    //        //Davice changed events subsription
    //        foreach (var device in DeviceListViewModel.Devices)
    //        {
    //            _testPingSender.DeviceChanged += device.HandleDeviceChanged;
    //        }
    //    }
    //    #endregion
    //}
    public partial class MainWindow : Window
    {
        public MainWindow(object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }
    }
}