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
        private readonly StatusStore _statusStore;
        private ObservableCollection<DeviceDTO> _devices;
        public ObservableCollection<DeviceDTO> Devices
        {
            get => _devices;
            set
            {
                _devices = value;
                OnPropertyChanged(nameof(Devices));
            }
        }
        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex == value)
                    return;
                _selectedIndex = value;
                OnSelectedIndexChange();
                OnPropertyChanged(nameof(SelectedIndex));
                OnPropertyChanged(nameof(SelectedDevice));
            }
        }
        private DeviceDTO? _selectedDevice;
        public DeviceDTO? SelectedDevice
        {
            get => _selectedDevice;
        }
        private bool _isAppNotBusy;
        public bool IsAppNotBusy => _isAppNotBusy;
        public DeviceListViewModel(DeviceListStore deviceStore, StatusStore statusStore)
        {
            _statusStore = statusStore;
            _deviceStore = deviceStore;
            _devices = new(_deviceStore.DeviceList);
            _isAppNotBusy = true;

            _deviceStore.Loaded += OnLoad;
            _deviceStore.Updated += OnUpdate;
            _statusStore.StatusChanged += StatusStore_StatusChanged;
        }

        private void StatusStore_StatusChanged()
        {
            _isAppNotBusy = !_statusStore.IsAppBusy;
            OnPropertyChanged(nameof(IsAppNotBusy));
        }

        public override void Dispose()
        {
            if (_deviceStore != null) _deviceStore.Loaded -= OnLoad;
            if (_deviceStore != null) _deviceStore.Updated -= OnUpdate;
            base.Dispose();
        }
        private void OnLoad(List<DeviceDTO> deviceList)
        {
            Devices = new(deviceList);
            _deviceStore.SubscribeDevicesChanged();
        }
        private void OnUpdate(List<DeviceDTO> deviceList)
        {
            Devices = new(deviceList);
        }
        private void OnSelectedIndexChange()
        {
            if (_selectedIndex >= 0 && Devices.Count >= _selectedIndex + 1)
            {
                _selectedDevice = Devices[_selectedIndex];
            }
        }
    }
}
