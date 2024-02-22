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
using System.Collections.Specialized;

namespace PingApp.ViewModels
{
    public class LoggsViewModel : ViewModelBase
    {
        public ObservableCollection<LogEvent> LogItemsSorted { get; set; }
        public ObservableCollection<LogEvent> LogItems
        {
            get;
        }
        public LoggsViewModel()
        {
            LogItemsSorted = [];
            LogItems = [];
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Observers(events =>
                        events.Do(evt => LogItems.Add(evt))
                              .Subscribe())
                              .CreateLogger();
            LogItems.CollectionChanged += LogItems_CollectionChanged;
        }

        private void LogItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (sender is ObservableCollection<LogEvent> collection)
                {
                    if (collection.Count > 0)
                    {
                        LogItemsSorted.Insert(0, collection.LastOrDefault());
                        return;
                    }
                    LogItemsSorted.Clear();
                }

            }
        }
    }
}
