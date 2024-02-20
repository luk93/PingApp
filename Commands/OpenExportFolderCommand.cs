using PingApp.DbServices;
using PingApp.Models;
using PingApp.Services;
using PingApp.Stores;
using PingApp.Tools;
using PingApp.ViewModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Commands
{
    public class OpenExportFolderCommand(DeviceListStore deviceStore) : CommandBase
    {
        private readonly DeviceListStore _deviceStore = deviceStore;

        public override void Execute(object? parameter)
        {
            try
            {
                Process.Start("explorer.exe", _deviceStore.XlsxExportPath);
            }
            catch (Exception ex)
            {
                var msg = $"Error message: {ex.Message}, Stack: {ex.StackTrace}";
                Log.Error(msg);
            }
        }
        
    }
}
