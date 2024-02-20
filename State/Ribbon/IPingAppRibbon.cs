using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PingApp.State.Ribbon
{
    public interface IPingAppRibbon
    {
        public ICommand? TriggerAllCommand { get;}
        public ICommand? GetDevicesFromExcelCommand { get;}
        public ICommand? ChangeExportPathCommand { get;}
        public ICommand? ExportDevicesToExcelCommand { get;}
    }
}
