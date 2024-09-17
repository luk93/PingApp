using AutoMapper;
using OfficeOpenXml;
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
using System.Windows.Interop;

namespace PingApp.Commands
{
    public class ExportDevicesWithSelHistoryToExcelCommand : ExportDevicesWithHistoryToExcelCommand
    {
        private readonly DeviceListViewModel _deviceListViewModel;
        public ExportDevicesWithSelHistoryToExcelCommand(DeviceListStore deviceStore, StatusStore statusStore, IMapper mapper, DeviceListViewModel deviceListViewModel) : base(deviceStore, statusStore, mapper)
        {
            _deviceListViewModel = deviceListViewModel;
        }

        

        public override async Task ExecuteAsync(object? parameter)
        {
            if (!base.IsXlsxExportPathSelected())
                return;
            var xlsxFilePath = $"{_deviceStore.XlsxExportPath}\\DevicesPingStatus_{FileTools.GetDateTimeString()}.xlsx";
            var excelPackage = CreateExcelPackage(xlsxFilePath);
            if (excelPackage == null)
                return;
            try
            {
                CreateAndStyleDeviceListSheet(excelPackage);
                CreateAndStyleSelectedHistorySheet(excelPackage);
                await ExcelManager.SaveExcelFile(excelPackage);
                var msg = $"Successfully created (.xlsx) file '{xlsxFilePath}'";
                Log.Information(msg);
            }
            catch (Exception ex)
            {
                var msg = $"Error message: {ex.Message}, Stack: {ex.StackTrace}";
                Log.Error(msg);
            }
        }
        private void CreateAndStyleSelectedHistorySheet(ExcelPackage excelPackage) 
        {
            if (_deviceListViewModel.SelectedDevice == null)
            {
                var msg = $"No item was selected!";
                Log.Warning(msg);
                return;
            }
            CreateAndStyleHistorySheet(excelPackage, _deviceListViewModel.SelectedDevice);
            Log.Information($"Created sheet with Ping History for selected '{_deviceListViewModel.SelectedDevice.Name}' device");
        }
    }
}
