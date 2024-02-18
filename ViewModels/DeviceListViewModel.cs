
using PingApp.Commands;
using PingApp.Models;
using PingApp.Stores;
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

namespace PingApp.ViewModels
{
    public class DeviceListViewModel : ViewModelBase
    {
        private ObservableCollection<DeviceViewModel> _deviceViewModels;
        ObservableCollection<DeviceViewModel> Devices
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
        public DeviceListViewModel(DeviceListStore deviceStore)
        {
            Devices = new ObservableCollection<DeviceViewModel>(deviceStore.DeviceList.Select(device => new DeviceViewModel(device)));
        }
        public void UpdateDevices(DeviceListStore deviceStore) 
        {
            Devices = new ObservableCollection<DeviceViewModel>(deviceStore.DeviceList.Select(device => new DeviceViewModel(device)));
        }
       
    }
}
