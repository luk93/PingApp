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

            //Style selector test - to delete after adjusting styles
            //
            //Log.Information("Logging started");
            //Log.Debug("Debug test");
            //Log.Verbose("Verbose test");
            //Log.Warning("Warning test");
            //Log.Error("Error test");
            //Log.Fatal("Fatal test");
        }
    }
}
