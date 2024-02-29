using PingApp.Models.Base;
using System;
using System.Collections.Generic;
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
    public partial class DeviceDTO : ObservableBaseModel
    {
        private string? _name;
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
        private PingStatus _status;
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
        private IPStatus _lastIpStatus;
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
        private string? _ipString;
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
        private DateTime _lastReplyDt;
        public DateTime LastReplyDt
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
        public List<PingResult> PingResults { get; set; } = [];
        
        public DeviceDTO(string? name, string? ipString)
        {
            _name = name;
            _ipString = ipString;
            _status = PingStatus.None;
            _lastIpStatus = IPStatus.Unknown;
            _lastReply = null;
            _lastReplyDt = DateTime.MinValue;
        }
        
    }
}
