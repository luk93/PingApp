using PingApp.Models;
using PingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static PingApp.Models.Device;

namespace PingApp.Services
{
    class DeviceListService
    {
        private ObservableCollection<Device> _deviceList;
        public DeviceListService(ObservableCollection<Device> deviceList)
        {
            _deviceList = deviceList;
        }
        public void FillDeviceList()
        {
            _deviceList.Clear();
            _deviceList.Add(new Device()
            {
                Name = "CS1",
                IpString = "192.168.1.31",
                Status = PingStatus.None,
                IpStatus = IPStatus.Unknown,
            });
            _deviceList.Add(new Device()
            {
                Name = "CS2",
                IpString = "192.168.1.32",
                Status = PingStatus.None,
                IpStatus = IPStatus.Unknown,
            });
            _deviceList.Add(new Device()
            {
                Name = "CS3",
                IpString = "192.168.1.33",
                Status = PingStatus.None,
                IpStatus = IPStatus.Unknown,
            });
            _deviceList.Add(new Device()
            {
                Name = "Camera",
                IpString = "192.168.1.64",
                Status = PingStatus.None,
                IpStatus = IPStatus.Unknown,
            });
        }
    }
}
