using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly DeviceListViewModel _deviceListViewModel;
        private readonly DevicePingSender _devicePingSender;
        private readonly DeviceListService _deviceListService;
        private readonly DeviceRecordService _deviceRecordService;
        private readonly DeviceListStore _deviceStore;

        public ICommand? TriggerAllCommand { get;}
        public ICommand? GetDevicesFromExcelCommand { get ;}

        public ICommand? ChangeExportPathCommand { get; }

        public ICommand? ExportDevicesToExcelCommand { get; }
        public ICommand? OpenExportFolderCommand { get; }

        public PingAppRibbon(ILogger logger, IMapper mapper, DeviceListViewModel deviceListViewModel, DevicePingSender devicePingSender, DeviceListService deviceListService, 
                            DeviceRecordService deviceRecordService, DeviceListStore deviceStore)
        {
            _logger = logger;
            _mapper = mapper;
            _deviceListViewModel = deviceListViewModel;
            _devicePingSender = devicePingSender;
            _deviceListService = deviceListService;
            _deviceRecordService = deviceRecordService;
            _deviceStore = deviceStore;
            TriggerAllCommand = new TriggerAllCommand(_deviceListViewModel, _devicePingSender);
            GetDevicesFromExcelCommand = new GetDevicesFromExcelCommand(_deviceStore, _logger, _deviceListService, _deviceRecordService);
            ChangeExportPathCommand = new ChangeExportPathCommand(_deviceStore);
            ExportDevicesToExcelCommand = new ExportDevicesToExcelCommand(_deviceStore, _logger, _mapper);
            OpenExportFolderCommand = new OpenExportFolderCommand(_deviceStore);
        }
    }
}
