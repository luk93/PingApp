using PingApp.Tools;
using PingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Commands
{
    public class TriggerAllCommand(DeviceListViewModel deviceListViewModel, DevicePingSender devicePingSender) : CommandBase
    {
        private readonly DeviceListViewModel _deviceListViewModel = deviceListViewModel;
        private readonly DevicePingSender _devicePingSender = devicePingSender;

        public override void Execute(object? parameter)
        {
            _devicePingSender.SendPingToDeviceList();
        }

        public override bool CanExecute(object? parameter)
        {
            return _deviceListViewModel != null && base.CanExecute(parameter);
        }
    }
}
