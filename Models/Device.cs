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
using static PingApp.Models.DeviceDb;

namespace PingApp.Models
{
    public partial class Device : ObservableBaseModel
    {
        private string _name;
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
        private IPStatus _ipStatus;
        public IPStatus IpStatus
        {
            get
            {
                return _ipStatus;
            }
            set
            {
                if (_ipStatus != value)
                {
                    _ipStatus = value;
                    OnPropertyChanged(nameof(IpStatus));
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
        
        public Device(string? name, string? ipString)
        {
            Name = name;
            IpString = ipString;
            Status = PingStatus.None;
            IpStatus = IPStatus.Unknown;
            LastReply = null;
            LastReplyDt = DateTime.MinValue;
            LastReply = null;
        }
        
    }
}
