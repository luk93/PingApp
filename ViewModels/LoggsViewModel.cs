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
using PingApp.Stores;

namespace PingApp.ViewModels
{
    public class LoggsViewModel(LoggsStore loggsStore) : ViewModelBase
    {
        private readonly LoggsStore _loggsStore = loggsStore;
        public ObservableCollection<LogEvent> LogItemsSorted => _loggsStore.LogItemsSorted;
        public ObservableCollection<LogEvent> LogItems => _loggsStore.LogItems;
    }
}
