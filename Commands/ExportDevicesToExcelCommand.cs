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
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Commands
{
    public class ExportDevicesToExcelCommand(DeviceListStore deviceStore, ILogger logger) : AsyncCommandBase
    {
        private readonly DeviceListStore _deviceStore = deviceStore;
        private readonly ILogger _logger = logger; 

        public override async Task ExecuteAsync(object? parameter)
        {
            if(!Directory.Exists(_deviceStore.XlsxExportPath))
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
            var xlsxFilePath = $"{_deviceStore.XlsxExportPath}\\DevicesPingStatus_{FileTools.GetDateTimeString()}.xlsx"; 
            var excelPackage = ExcelManager.CreateExcelFile(xlsxFilePath);
            if (excelPackage == null)
            {
                var msg = $"Failed to create (.xlsx) file '{xlsxFilePath}'!";
                Log.Error(msg);
                _logger.Error(msg);
                return;
            }
            try
            {
                var ws = excelPackage.Workbook.Worksheets.Add("DeviceList");
                var range = ws.Cells["A1"].LoadFromCollection(_deviceStore.DeviceList, true);
                range.AutoFitColumns();
                await ExcelManager.SaveExcelFile(excelPackage);
                var msg = $"Successfully created (.xlsx) file '{xlsxFilePath}'!";
                Log.Information(msg);
            }
            catch (Exception ex)
            {
                var msg = $"Error message: {ex.Message}, Stack: {ex.StackTrace}";
                Log.Error(msg);
            }
        }
        
    }
}
