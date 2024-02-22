using OfficeOpenXml.Drawing.Controls;
using PingApp.Models;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using static PingApp.Models.DeviceDb;

namespace PingApp.Converters
{
    public class DeviceStatusToStyleConverter : IValueConverter
    {
        //public Style? BusyStyle { get; set; }
        //public Style? SuccessStyle { get; set; }
        //public Style? FailedStyle { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not PingStatus status) return null;
            return status switch
            {
                PingStatus.Busy => new SolidColorBrush(Colors.FloralWhite),
                PingStatus.Failure => new SolidColorBrush(Colors.LightCoral),
                PingStatus.Success => new SolidColorBrush(Colors.LightGreen),
                _ => null,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
