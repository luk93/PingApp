using PingApp.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static PingApp.Models.DeviceDTO;

namespace PingApp.Models
{
    public class Device : DbSetBaseModel
    {
        public string? Name { get; set; }
        public PingStatus Status { get; set; }
        public IPAddress? IpAddress
        {
            get
            {
                return Tools.Converters.ConvertStrToIpAddress(IpString);
            }
            set { }
        }

        public string? IpString { get; set; }
        public IPStatus LastIpStatus { get; set; }
        public DateTime? LastReplyDt { get; set; }
        public enum PingStatus
        {
            None,
            Busy,
            Canceled,
            Success,
            Failure
        }
        //Relations
        public virtual List<PingResult> PingResults { get; set; } = [];
    }
}
