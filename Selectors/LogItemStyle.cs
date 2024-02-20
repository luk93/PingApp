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
            if (item is not LogEvent logEvent) return null;

            return logEvent.Level switch
            {
                LogEventLevel.Verbose => VerboseStyle,
                LogEventLevel.Debug => DebugStyle,
                LogEventLevel.Information => InformationStyle,
                LogEventLevel.Warning => WarningStyle,
                LogEventLevel.Error => ErrorStyle,
                LogEventLevel.Fatal => FatalStyle,
                _ => null,
            };
        }
    }
}
