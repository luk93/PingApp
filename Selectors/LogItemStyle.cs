using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace PingApp.Selectors
{
    public class LogItemStyle : StyleSelector
    {
        public Style? VerboseStyle { get; set; }
        public Style? DebugStyle { get; set; }
        public Style? InformationStyle { get; set; }
        public Style? WarningStyle { get; set; }
        public Style? ErrorStyle { get; set; }
        public Style? FatalStyle { get; set; }
        public override Style? SelectStyle(object item, DependencyObject container)
        {
            LogEvent? logEvent = item as LogEvent;
            if(logEvent == null) return null;

            switch (logEvent.Level)
            {
                case LogEventLevel.Verbose:
                    return VerboseStyle;
                case LogEventLevel.Debug:
                    return DebugStyle;
                case LogEventLevel.Information:
                    return InformationStyle;
                case LogEventLevel.Warning:
                    return WarningStyle;
                case LogEventLevel.Error:
                    return ErrorStyle;
                case LogEventLevel.Fatal:
                    return FatalStyle;
                default:
                    return null;
            }
        }
    }
}
