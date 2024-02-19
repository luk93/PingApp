using Microsoft.Extensions.Logging;
using PingApp.ViewModels;
using Serilog.Events;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PingApp.State.StatusBar
{
    public class PingAppStatusBar : IPingAppStatusBar
    {
        private readonly LoggsViewModel _loggsViewModel;
        public ObservableCollection<LogEvent> LogItems => _loggsViewModel.LogItems;
        public string LastLogItem => LogItems?.LastOrDefault()?.MessageTemplate?.ToString() ?? string.Empty;

        public ICommand? Command => throw new NotImplementedException();

        public PingAppStatusBar(LoggsViewModel loggsViewModel)
        {
            _loggsViewModel = loggsViewModel;
        }
    }
}
