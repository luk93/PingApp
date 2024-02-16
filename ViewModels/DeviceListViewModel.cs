
using PingApp.Models;
using PingApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PingApp.ViewModel
{
    class DeviceListViewModel : ViewModelBase
    {
        private ObservableCollection<DeviceViewModel> _deviceViewModels;
        public ObservableCollection<DeviceViewModel> Devices
        {
            get
            {
                return _deviceViewModels;
            }
            set
            {
                _deviceViewModels = value;
                OnPropertyChanged(nameof(Devices));
            }
        }
        public bool CanTrigger => Devices.Count > 0;
        public ICommand? TriggerAllCommand {get;}
        public ICommand? GetDevicesFromExcelCommand {get;}

        public DeviceListViewModel(IEnumerable<Device> devices, ICommand? triggerAllCommand, ICommand? getDevicesFromExcelCommand)
        {
            Devices = new ObservableCollection<DeviceViewModel>(devices.Select(device => new DeviceViewModel(device)));
            TriggerAllCommand = triggerAllCommand;
            GetDevicesFromExcelCommand = getDevicesFromExcelCommand;
        }
        public void UpdateDevices(IEnumerable<Device> devices) 
        {
            Devices = new ObservableCollection<DeviceViewModel>(devices.Select(device => new DeviceViewModel(device)));
        }
       
    }
}
