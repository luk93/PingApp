using Serilog.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static OfficeOpenXml.ExcelErrorValue;
using PingApp.ViewModels.Base;
using System.Reactive.Linq;

namespace PingApp.ViewModels
{
    public class LoggsViewModel : ViewModelBase
    {
        public ObservableCollection<LogEvent> LogItems
        {
            get;
        }
        public LoggsViewModel()
        {
            LogItems = new ObservableCollection<LogEvent>();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Observers(events =>
                        events.Do(evt => LogItems.Add(evt))
                              .Subscribe())
                              .CreateLogger();
        }
    }
}
