using PingApp.Models;
using PingApp.Stores;
using PingApp.Tools;
using PingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Commands
{
    public class TriggerAllContCommand : AsyncCommandBase
    {
        private readonly DevicePingSender _devicePingSender;
        private readonly StatusStore _statusStore;
        private readonly DeviceListStore _deviceStore;

        public TriggerAllContCommand(DevicePingSender devicePingSender, StatusStore statusStore, DeviceListStore deviceStore)
        {
            _devicePingSender = devicePingSender;
            _statusStore = statusStore;
            _deviceStore = deviceStore;

            _deviceStore.AnyDeviceChanged += DeviceStore_AnyDeviceChanged;
            _statusStore.StatusChanged += StatusStore_StatusChanged;
        }

        private void DeviceStore_AnyDeviceChanged(DeviceDTO device,bool selectedToPing)
        {
            OnCanExecutedChanged();
        }

        private void StatusStore_StatusChanged()
        {
            OnCanExecutedChanged();
        }

        public override Task ExecuteAsync(object? parameter)
        {
            _devicePingSender.SendPingToDeviceList(true);
            return Task.CompletedTask;
        }

        public override bool CanExecute(object? parameter)
        {
            var selectedDevices = _deviceStore.DeviceList.Where(x => x.SelectedToPing);
            return !_statusStore.IsAppBusy && selectedDevices.Any() && base.CanExecute(parameter);
        }

        
    }
}
