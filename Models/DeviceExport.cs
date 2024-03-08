using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static PingApp.Models.Device;

namespace PingApp.Models
{
    public class DeviceExport
    {
        public string? Name { get; set; }
        public PingStatus Status { get; set; }
        public IPAddress? IpAddress { get; set; }
        public IPStatus LastIpStatus { get; set; }
        public DateTime LastReplyDt { get; set; }
    }
}
