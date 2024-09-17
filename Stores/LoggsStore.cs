using Serilog;
using Serilog.Core;
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
        public ObservableCollection<LogEvent> LogItems { get; }

        public LoggsStore()
        {
            _logItemsSorted = new ObservableCollection<LogEvent>();
            LogItems = new ObservableCollection<LogEvent>();

            var logSink = new ObservableCollectionSink(logEvent => LogItems.Add(logEvent));
            Log.Logger = new LoggerConfiguration()
                             .WriteTo.Logger(Log.Logger)
                             .WriteTo.Sink(logSink)
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
                            .LastOrDefault()?.MessageTemplate.ToString();
                        return;
                    }
                    _logItemsSorted.Clear();
                }
            }
        }

        private class ObservableCollectionSink : ILogEventSink
        {
            private readonly Action<LogEvent> _writeAction;

            public ObservableCollectionSink(Action<LogEvent> writeAction)
            {
                _writeAction = writeAction;
            }

            public void Emit(LogEvent logEvent)
            {
                _writeAction(logEvent);
            }
        }
    }
}
