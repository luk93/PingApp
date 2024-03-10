using PingApp.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Models
{
    public class PingResultExport
    {
        public IPStatus IpStatus { get; set; }
        public DateTime? ReplyDt { get; set; }
    }
}
