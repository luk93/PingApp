using PingApp.Commands;
using PingApp.Models;
using PingApp.Stores;
using PingApp.Tools;
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
        private DevicePingSender _pingSender;
        private DeviceListStore? _deviceStore;
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
        public DeviceListViewModel(DeviceListStore deviceStore, DevicePingSender devicePingSender)
        {
            _deviceStore = deviceStore;
            _pingSender = devicePingSender;
            UpdateDevices(_deviceStore.DeviceList);
            _deviceStore.Loaded += OnLoad;
            _deviceStore.Updated += OnUpdate;
        }
        public override void Dispose()
        {
            if(_deviceStore != null) _deviceStore.Loaded -= OnLoad;
            base.Dispose();
        }
        private void OnLoad(List<Device> deviceList)
        {
            UpdateDevices(deviceList);
        }
        private void OnUpdate(List<Device> deviceList)
        {
            UpdateDevices(deviceList);
        }
        public void UpdateDevices(List<Device> deviceList) 
        {
            Devices = new ObservableCollection<DeviceViewModel>(deviceList.Select(device => new DeviceViewModel(device)));
            foreach(var device in Devices) 
            {
                _pingSender.DeviceChanged += device.HandleDeviceChanged;
            }
        }

       
    }
}
