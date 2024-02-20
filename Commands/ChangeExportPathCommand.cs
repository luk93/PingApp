using PingApp.DbServices;
using PingApp.Models;
using PingApp.Services;
using PingApp.Stores;
using PingApp.Tools;
using PingApp.ViewModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Commands
{
    public class ChangeExportPathCommand(DeviceListStore deviceStore) : CommandBase
    {
        private readonly DeviceListStore _deviceStore = deviceStore;

        public override void Execute(object? parameter)
        {
            var openFolderDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            {
                openFolderDialog.Description = "Select export Directory:";
            };
            var result = openFolderDialog.ShowDialog();
            if (result == true)
            {
                _deviceStore.XlsxExportPath = openFolderDialog.SelectedPath;
                Log.Information($"Export path changed to '{_deviceStore.XlsxExportPath}'");
            }
        }
        
    }
}
