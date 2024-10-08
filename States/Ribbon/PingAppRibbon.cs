﻿using AutoMapper;
using PingApp.Commands;
using PingApp.DbServices;
using PingApp.Models;
using PingApp.Services;
using PingApp.States.Navigators;
using PingApp.States.Ribbon;
using PingApp.Stores;
using PingApp.Tools;
using PingApp.ViewModels;
using PingApp.ViewModels.Factories;
using Serilog;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PingApp.States.Ribbon
{
    public class PingAppRibbon : IPingAppRibbon
    {
        private readonly IMapper _mapper;
        private readonly StatusStore _statusStore;
        private readonly DevicePingSender _devicePingSender;
        private readonly DeviceListService _deviceListService;
        private readonly DeviceDbService _deviceDbService;
        private readonly DeviceListStore _deviceStore;
        private readonly DeviceListViewModel _deviceListViewModel;
        private readonly IPingAppViewModelFactory _viewModelFactory;
        private readonly IPingAppNavigator _navigator;

        public ICommand? TriggerAllCommand { get;}
        public ICommand? TriggerAllContCommand { get; }
        public ICommand? GetDevicesFromExcelCommand { get ;}
        public ICommand? ChangeExportPathCommand { get; }
        public ICommand? ExportDevicesWithoutHistoryToExcelCommand { get; }
        public ICommand? ExportDevicesWithHistoryToExcelCommand { get; }
        public ICommand? ExportDevicesWithSelHistoryToExcelCommand { get; }
        public ICommand? OpenExportFolderCommand { get; }
        public ICommand? CancelPingCommand { get; }
        public ICommand? UpdateCurrentViewModel {  get; }
        public ICommand? SelectAllDevicesCommand {  get; }
        public ICommand? UnselectAllDevicesCommand {  get; }

        public PingAppRibbon(IMapper mapper, StatusStore statusStore, DevicePingSender devicePingSender, DeviceListService deviceListService,
                            DeviceDbService deviceDbService, DeviceListStore deviceStore, IPingAppViewModelFactory viewModelFactory, IPingAppNavigator navigator,
                            DeviceListViewModel deviceListViewModel)
        {
            _mapper = mapper;
            _statusStore = statusStore;
            _devicePingSender = devicePingSender;
            _deviceListService = deviceListService;
            _deviceDbService = deviceDbService;
            _deviceStore = deviceStore;
            _deviceListViewModel = deviceListViewModel;
            _viewModelFactory = viewModelFactory;
            _navigator = navigator;
            TriggerAllCommand = new TriggerAllCommand(_devicePingSender, _statusStore, _deviceStore);
            TriggerAllContCommand = new TriggerAllContCommand(_devicePingSender, _statusStore, _deviceStore);
            GetDevicesFromExcelCommand = new GetDevicesFromExcelCommand(_deviceStore, _deviceListService, _deviceDbService);
            ChangeExportPathCommand = new ChangeExportPathCommand(_deviceStore);
            ExportDevicesWithoutHistoryToExcelCommand = new ExportDevicesWithoutHistoryToExcelCommand(_deviceStore, _statusStore, _mapper);
            ExportDevicesWithHistoryToExcelCommand = new ExportDevicesWithHistoryToExcelCommand(_deviceStore, _statusStore, _mapper);
            ExportDevicesWithSelHistoryToExcelCommand = new ExportDevicesWithSelHistoryToExcelCommand(_deviceStore, _statusStore, _mapper, _deviceListViewModel);
            OpenExportFolderCommand = new OpenExportFolderCommand(_deviceStore);
            UpdateCurrentViewModel = new UpdateCurrentViewModelCommand(_navigator, _viewModelFactory);
            CancelPingCommand = new CancelPingCommand(_devicePingSender, _statusStore);
            SelectAllDevicesCommand = new SelectAllDevicesCommand(_deviceStore, _statusStore);
            UnselectAllDevicesCommand = new UnselectAllDevicesCommand(_deviceStore, _statusStore);
        }
    }
}
