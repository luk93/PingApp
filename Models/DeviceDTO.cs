using PingApp.Models.Base;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static PingApp.Models.Device;

namespace PingApp.Models
{
    public partial class DeviceDTO(string? name, string? ipString) : ObservableBaseModel
    {
        private string? _name = name;
        public string? Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        private PingStatus _status = PingStatus.None;
        public PingStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }
        private IPStatus _lastIpStatus = IPStatus.Unknown;
        public IPStatus LastIpStatus
        {
            get
            {
                return _lastIpStatus;
            }
            set
            {
                if (_lastIpStatus != value)
                {
                    _lastIpStatus = value;
                    OnPropertyChanged(nameof(LastIpStatus));
                }
            }
        }
        private string? _ipString = ipString;
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
                    OnPropertyChanged(nameof(IpAddress));
                }
            }
        }
        public IPAddress? IpAddress
        {
            get
            {
                return Tools.Converters.ConvertStrToIpAddress(IpString);
            }
            private set { }
        }
        private DateTime? _lastReplyDt;
        public DateTime? LastReplyDt
        {
            get
            {
                return _lastReplyDt;
            }
            set
            {
                if (_lastReplyDt != value)
                {
                    _lastReplyDt = value;
                    OnPropertyChanged(nameof(LastReplyDt));
                }
            }
        }
        private PingReply? _lastReply;
        public PingReply? LastReply
        {
            get
            {
                return _lastReply;
            }
            set
            {
                if (_lastReply != value)
                {
                    _lastReply = value;
                    OnPropertyChanged(nameof(LastReply));
                }
            }
        }
        private bool _selectedToPing = false;
        public bool SelectedToPing
        {
            get
            {
                return _selectedToPing;
            }
            set
            {
                if (_selectedToPing != value)
                {
                    _selectedToPing = value;
                    OnPropertyChanged(nameof(SelectedToPing));
                    DeviceChanged?.Invoke(this,value);
                }
            }
        }
        private int _pingCount = 0;
        public int PingCount
        {
            get
            {
                return _pingCount;
            }
            set
            {
                if (_pingCount != value)
                {
                    _pingCount = value;
                    OnPropertyChanged(nameof(PingCount));
                }
            }
        }
        private int _timeout = 0;
        public int Timeout
        {
            get
            {
                return _timeout;
            }
            set
            {
                if (_timeout != value)
                {
                    _timeout = value;
                    OnPropertyChanged(nameof(Timeout));
                }
            }
        }
        public ObservableCollection<PingResult> PingResults {get;set;} = [];

        public delegate void DeviceChangedEventHandler(DeviceDTO device, bool selectedToPing);
        public event DeviceChangedEventHandler? DeviceChanged;
    }
}
