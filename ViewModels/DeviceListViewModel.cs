
using PingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

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
        public DeviceListViewModel(IEnumerable<Device> devices)
        {
            Devices = new ObservableCollection<DeviceViewModel>(devices.Select(device => new DeviceViewModel(device)));
        }
    }
}
