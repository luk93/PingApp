using PingApp.Commands;
using PingApp.DbServices;
using PingApp.Models;
using PingApp.Services;
using PingApp.State.Ribbon;
using PingApp.Stores;
using PingApp.Tools;
using PingApp.ViewModels;
using Serilog;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PingApp.Ribbon
{
    public class PingAppRibbon : IPingAppRibbon
    {
        private readonly ILogger _logger;
        private DeviceListViewModel _deviceListViewModel;
        private DevicePingSender _devicePingSender;
        private DeviceListService _deviceListService;
        private DeviceRecordService _deviceRecordService;
        private DeviceListStore _deviceStore;

        public ICommand? TriggerAllCommand { get;}
        public ICommand? GetDevicesFromExcelCommand { get ;}

        public PingAppRibbon(ILogger logger, DeviceListViewModel deviceListViewModel, DevicePingSender devicePingSender, 
                            DeviceListService deviceListService, DeviceRecordService deviceRecordService, DeviceListStore deviceStore)
        {
            _logger = logger;
            _deviceListViewModel = deviceListViewModel;
            _devicePingSender = devicePingSender;
            _deviceListService = deviceListService;
            _deviceRecordService = deviceRecordService;
            _deviceStore = deviceStore;
            TriggerAllCommand = new TriggerAllCommand(_deviceListViewModel, _devicePingSender);
            GetDevicesFromExcelCommand = new GetDevicesFromExcelCommand(_deviceStore, _logger, _deviceListService, _deviceListViewModel, _deviceRecordService); ;
           
        }
    }
}
