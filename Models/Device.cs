using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Models
{
    class Device
    {
        public string Name {  get; set; }
        public PingStatus Status { get; set; }
        public IPStatus IpStatus { get; set; }
        public IPAddress? IpAddress
        {
            get
            {
                return ConvertStrToIpAddress(IpString);
            }
            private set { }
        }
        public bool IsBusy { get; set; }
        public string IpString { get; set; }
        public DateTime LastReplyDt { get; set; }
        public PingReply? LastReply { get; set; }
        public enum PingStatus
        {
            None,
            Waiting,
            Canceled,
            Success,
            Failure
        }
        public Device(string name, string ipString)
        {
            Name = name;
            IpString = ipString;
            Status = PingStatus.None;
            IpStatus = IPStatus.Unknown;
            LastReply = null;
            LastReplyDt = DateTime.MinValue;
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
    }
}
