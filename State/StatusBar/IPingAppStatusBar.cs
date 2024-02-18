using Microsoft.Extensions.Logging;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PingApp.State.StatusBar
{
    public interface IPingAppStatusBar
    {
        public ObservableCollection<LogEvent> LogItems { get; }
        public string LastLogItem { get; }
        ICommand? Command { get;}
    }
}
