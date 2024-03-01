using OfficeOpenXml.Drawing.Controls;
using PingApp.Models;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using static PingApp.Models.Device;

namespace PingApp.Converters
{
    public class IpStatusToColorConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not IPStatus status) return null;
            return status switch
            {
                IPStatus.Unknown => new SolidColorBrush(Colors.LightSkyBlue),
                IPStatus.Success => new SolidColorBrush(Colors.LightGreen),
                _ => new SolidColorBrush(Colors.LightCoral),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
