using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PingApp.States.Ribbon
{
    public interface IPingAppRibbon
    {
        public ICommand? TriggerAllCommand { get; }
        public ICommand? GetDevicesFromExcelCommand { get; }
        public ICommand? ChangeExportPathCommand { get; }
        public ICommand? ExportDevicesWithoutHistoryToExcelCommand { get; }
        public ICommand? ExportDevicesWithHistoryToExcelCommand { get; }
        public ICommand? ExportDevicesWithSelHistoryToExcelCommand { get; }
        public ICommand? OpenExportFolderCommand { get; }
        public ICommand? CancelPingCommand { get; }
        public ICommand? UpdateCurrentViewModel { get; }
        public ICommand? SelectAllDevicesCommand { get; }
        public ICommand? UnselectAllDevicesCommand { get; }
    }
}
