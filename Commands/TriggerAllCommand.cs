using PingApp.Tools;
using PingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Commands
{
    public class TriggerAllCommand : CommandBase
    {
        private readonly DeviceListViewModel _deviceListViewModel;
        private readonly DevicePingSender _devicePingSender;

        public TriggerAllCommand(DeviceListViewModel deviceListViewModel, DevicePingSender devicePingSender)
        {
            _deviceListViewModel = deviceListViewModel;
            _devicePingSender = devicePingSender;
        }

        public override void Execute(object parameter)
        {
            _devicePingSender.SendPingToDeviceList();
        }

        public override bool CanExecute(object? parameter)
        {
            return _deviceListViewModel != null && base.CanExecute(parameter);
        }
    }
}
