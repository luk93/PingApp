using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using PingApp.ViewModels;
using PingApp.Models;

namespace PingApp.Selectors
{
    public class DeviceItemStyle : StyleSelector
    {
        public Style? IsBusyStyle { get; set; }
        public Style? SuccessStyle { get; set; }
        public Style? FailedStyle { get; set; }
        
        public override Style? SelectStyle(object item, DependencyObject container)
        {
            if (item is not Device device) return null;

            if(device.IsBusy) return IsBusyStyle;
            if(device.Status == DeviceDb.PingStatus.Success) return SuccessStyle;
            if(device.Status == DeviceDb.PingStatus.Failure) return FailedStyle;
            return null;
        }
    }
}
