using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Stores
{
    public class LoggsStore
    {
        private string? _lastLogItem;
        private readonly ObservableCollection<LogEvent> _logItemsSorted;
        public string? LastLogItem => _lastLogItem;
        public ObservableCollection<LogEvent> LogItemsSorted => _logItemsSorted;
        public ObservableCollection<LogEvent> LogItems
        {
            get;
        }
        public LoggsStore()
        {
            _logItemsSorted = [];
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
                        _logItemsSorted.Insert(0, collection.LastOrDefault());
                        _lastLogItem = collection
                            .Where(x => x.Level < LogEventLevel.Error)
                            .LastOrDefault().MessageTemplate.ToString();
                        return;
                    }
                    _logItemsSorted.Clear();
                }

            }
        }

    }
}
