using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static PingApp.ViewModel.Device;

namespace PingApp.ViewModel
{
    class DeviceList : ViewModelBase
    {
        public ObservableCollection<Device> Devices
        {
            get
            {
                return _devices;
            }
            set
            {
                _devices = value;
                OnPropertyChanged(nameof(_devices));
            }
        }
        private ObservableCollection<Device> _devices;

        public DeviceList()
        {
            _devices = [];
        }
        public void FillDeviceList()
        {
            _devices.Clear();
            _devices.Add(new Device()
            {
                Name = "CS1",
                IpString = "192.168.1.31",
                Status = PingStatus.None,
                IPStatus = IPStatus.Unknown,
            });
            _devices.Add(new Device()
            {
                Name = "CS2",
                IpString = "192.168.1.32",
                Status = PingStatus.None,
                IPStatus = IPStatus.Unknown,
            });
            _devices.Add(new Device()
            {
                Name = "CS3",
                IpString = "192.168.1.33",
                Status = PingStatus.None,
                IPStatus = IPStatus.Unknown,
            });
            _devices.Add(new Device()
            {
                Name = "Camera",
                IpString = "192.168.1.64",
                Status = PingStatus.None,
                IPStatus = IPStatus.Unknown,
            });
        }
    }
}
