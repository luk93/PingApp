using Microsoft.VisualBasic.Logging;
using PingApp.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Models
{
    public class PingResult : DbSetBaseModel
    {
        public IPStatus IpStatus { get; set; }
        public DateTime? ReplyDt { get; set; }
        public long? RoundTripTime { get; set; }
        public int? TimeToLive { get; set; }
        public int? BufferSizeSent { get; set; }
        public int? BufferSizeReceived { get; set; }
        //Relations
        public int? DeviceId { get; set; }
        public virtual Device? Device { get; set; }
    }
}
