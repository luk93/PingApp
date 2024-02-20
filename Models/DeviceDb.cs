using PingApp.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static PingApp.Models.Device;

namespace PingApp.Models
{
    public class DeviceDb : DbSetBaseModel
    {
        public string? Name { get; set; }
        public PingStatus Status { get; set; }
        public IPStatus IpStatus { get; set; }
        public IPAddress? IpAddress
        {
            get
            {
                return ConvertStrToIpAddress(IpString);
            }
            set { }
        }

        public string? IpString { get; set; }
        public DateTime LastReplyDt { get; set; }
        public enum PingStatus
        {
            None,
            Waiting,
            Canceled,
            Success,
            Failure
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
