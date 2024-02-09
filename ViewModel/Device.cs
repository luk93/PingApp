using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PingApp.ViewModel
{
    class Device : ViewModelBase
    {
        private string _name;
        private PingStatus _status;
        private IPStatus _ipStatus;
        private bool _isBusy;
        private string _ipString;
        private DateTime _lastReplyDt;
        private PingReply _lastReply;

        public enum PingStatus
        {
            None,
            Waiting,
            Canceled,
            Success,
            Failure
        }
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
        public IPStatus IPStatus
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
                    OnPropertyChanged(nameof(IPStatus));
                }
            }
        }
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
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

        private IPAddress? ConvertStrToIpAddress(string? ipString)
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
    }
}
