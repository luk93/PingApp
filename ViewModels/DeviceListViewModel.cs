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
        private readonly DeviceListStore? _deviceStore;
        private ObservableCollection<Device> _devices;
        public ObservableCollection<Device> Devices
        {
            get => _devices;
            set
            {
                _devices = value;
                OnPropertyChanged(nameof(Devices));
            }
        }
        public bool CanTrigger => Devices.Count > 0;
        public DeviceListViewModel(DeviceListStore deviceStore)
        {
            _deviceStore = deviceStore;
            _devices = new(_deviceStore.DeviceList);

            _deviceStore.Loaded += OnLoad;
            _deviceStore.Updated += OnUpdate;
        }
        public override void Dispose()
        {
            if(_deviceStore != null) _deviceStore.Loaded -= OnLoad;
            if(_deviceStore != null) _deviceStore.Updated -= OnUpdate;
            base.Dispose();
        }
        private void OnLoad(List<Device> deviceList)
        {
            Devices = new(deviceList);
        }
        private void OnUpdate(List<Device> deviceList)
        {
            Devices = new(deviceList);
        }       
    }
}
