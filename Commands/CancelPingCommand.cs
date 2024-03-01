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
    public class CancelPingCommand : AsyncCommandBase
    {
        private readonly DevicePingSender _devicePingSender;
        private readonly StatusStore _statusStore;
        public CancelPingCommand(DevicePingSender devicePingSender, StatusStore statusStore)
        {
            _devicePingSender = devicePingSender;
            _statusStore = statusStore;
            _statusStore.StatusChanged += StatusStore_StatusChanged;
        }

        private void StatusStore_StatusChanged()
        {
            OnCanExecutedChanged();
        }

        public override Task ExecuteAsync(object? parameter)
        {
            _devicePingSender.CancelPingToDeviceList();
            return Task.CompletedTask;
        }

        public override bool CanExecute(object? parameter)
        {
            return _statusStore.IsAppBusy && base.CanExecute(parameter);
        }

        
    }
}
