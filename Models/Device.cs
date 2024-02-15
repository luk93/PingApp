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

namespace PingApp.Models
{
    public partial class Device : DeviceDb
    {
        public bool IsBusy { get; set; }
        public PingReply? LastReply { get; set; }

        public Device(string name, string ipString)
        {
            Name = name;
            IpString = ipString;
            Status = PingStatus.None;
            IpStatus = IPStatus.Unknown;
            LastReply = null;
            LastReplyDt = DateTime.MinValue;
            IsBusy = false;
            LastReply = null;
        }
        public Device(DeviceDb deviceDb) 
        {
            Id = deviceDb.Id;
            Name = deviceDb.Name;
            IpString = deviceDb.IpString;
            Status = deviceDb.Status;
            IpStatus = deviceDb.IpStatus;
            IpAddress = deviceDb.IpAddress;
            LastReplyDt = deviceDb.LastReplyDt;
            IsBusy = false; 
            LastReply = null;
        }
        
    }
}
