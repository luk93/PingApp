using AutoMapper;
using OfficeOpenXml;
using PingApp.DbServices;
using PingApp.Models;
using PingApp.Services;
using PingApp.Stores;
using PingApp.Tools;
using PingApp.ViewModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Commands
{
    public class UnselectAllDevicesCommand : CommandBase
    {
        private readonly DeviceListStore _deviceStore;
        private readonly StatusStore _statusStore;
        public UnselectAllDevicesCommand(DeviceListStore deviceStore, StatusStore statusStore)
        {
            _deviceStore = deviceStore;
            _statusStore = statusStore;

            _statusStore.StatusChanged += StatusStore_StatusChanged;
        }

        private void StatusStore_StatusChanged()
        {
            OnCanExecutedChanged();
        }
        public override void Execute(object? parameter)
        {
            foreach (var device in _deviceStore.DeviceList)
            {
                device.SelectedToPing = false;
            }
            _deviceStore.Update(_deviceStore.DeviceList);
        }
        public override bool CanExecute(object? parameter)
        {
            return _deviceStore.DeviceList.Count > 0 && !_statusStore.IsAppBusy && base.CanExecute(parameter);
        }

    }
}
