using PingApp.Models;
using PingApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using static PingApp.Models.Device;
using static PingApp.Models.DeviceDb;

namespace PingApp.ViewModels
{
    public class DeviceViewModel: ViewModelBase, IObserver
    {
        private Device _device;
        private string _ipString;
        public string? Name
        {
            get
            {
                return _device.Name;
            }
            set
            {
                if (_device.Name != value)
                {
                    _device.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        public PingStatus Status
        {
            get
            {
                return _device.Status;
            }
            set
            {
                if (_device.Status != value)
                {
                    _device.Status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }
        public IPStatus IpStatus
        {
            get
            {
                return _device.IpStatus;
            }
            set
            {
                if (_device.IpStatus != value)
                {
                    _device.IpStatus = value;
                    OnPropertyChanged(nameof(IpStatus));
                }
            }
        }
        public bool IsBusy
        {
            get
            {
                return _device.IsBusy;
            }
            set
            {
                if (_device.IsBusy != value)
                {
                    _device.IsBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }
        public string? IpString
        {
            get
            {
                return _ipString;
            }
            set
            {
                if (_ipString != value)
                {
                    _ipString = value;
                    OnPropertyChanged(nameof(IpString));
                }
            }
        }
        public IPAddress? IpAddress
        {
            get
            {
                return ConvertStrToIpAddress(IpString);
            }
            private set { }
        }
        public DateTime LastReplyDt
        {
            get
            {
                return _device.LastReplyDt;
            }
            set
            {
                if (_device.LastReplyDt != value)
                {
                    _device.LastReplyDt = value;
                    OnPropertyChanged(nameof(LastReplyDt));
                }
            }
        }

        public PingReply? LastReply
        {
            get
            {
                return _device.LastReply;
            }
            set
            {
                if (_device.LastReply != value)
                {
                    _device.LastReply = value;
                    OnPropertyChanged(nameof(LastReply));
                }
            }
        }
        private static IPAddress? ConvertStrToIpAddress(string? ipString)
        {
            if (ipString == null) return null;
            try
            {
                return IPAddress.Parse(ipString);
            }
            catch
            {
                return null;
            }
        }

        public DeviceViewModel(Device device)
        {
            _device = device;
            Name = device.Name;
            Status = device.Status;
            IpStatus = device.IpStatus;
            IpAddress = device.IpAddress;
            IpString = device.IpString;
            IsBusy = device.IsBusy;
            LastReplyDt = device.LastReplyDt;
            LastReply = device.LastReply;
        }
        public void HandleDeviceChanged(Object sender, EventArgs args)
        {
            if (sender is Device changedDevice && ReferenceEquals(changedDevice, _device))
            {
                Name = changedDevice.Name;
                Status = changedDevice.Status;
                IpStatus = changedDevice.IpStatus;
                IpAddress = changedDevice.IpAddress;
                IpString = changedDevice.IpString;
                IsBusy = changedDevice.IsBusy;
                LastReplyDt = changedDevice.LastReplyDt;
                LastReply = changedDevice.LastReply;
                OnPropertyChanged(null);
            }
        }
    }
}
