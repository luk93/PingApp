using PingApp.Stores;
using PingApp.ViewModels.Base;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PingApp.ViewModels
{
    public class ConfigViewModel: ViewModelBase
    {
        private readonly string _dummy;
        public string Dummy => _dummy;
        public ConfigViewModel()
        {
            _dummy = "dummy";
        }
    }
}
