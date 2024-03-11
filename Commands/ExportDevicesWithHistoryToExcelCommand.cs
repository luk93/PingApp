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
    public class ExportDevicesWithHistoryToExcelCommand : ExportDevicesToExcelCommand
    {
        public ExportDevicesWithHistoryToExcelCommand(DeviceListStore deviceStore, StatusStore statusStore, ILogger logger, IMapper mapper) : base(deviceStore, statusStore, logger, mapper)
        {
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
                CreateAndStyleHistorySheets(excelPackage);
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
        private void CreateAndStyleHistorySheets(ExcelPackage excelPackage) 
        {
            foreach(var device in _deviceStore.DeviceList)
            {
                CreateAndStyleHistorySheet(excelPackage, device);
            }
            Log.Information($"Created sheets with Ping History for {_deviceStore.DeviceList.Count} devices");
        }
        protected void CreateAndStyleHistorySheet(ExcelPackage excelPackage, DeviceDTO device) 
        {
            try
            {
                var ws = excelPackage.Workbook.Worksheets.Add(device.Name);
                var range = ws.Cells["A1"].LoadFromCollection(_mapper.Map<List<PingResultExport>>(device.PingResults), true);
                range.AutoFitColumns();
                StyleHistoryWorksheet(ws);
            }
            catch(Exception ex) 
            {
                var msg = $"Error message: {ex.Message}, Stack: {ex.StackTrace}";
                Log.Error(msg);
            }
        }
        protected static void StyleHistoryWorksheet(ExcelWorksheet ws)
        {
            ws.Column(2).Style.Numberformat.Format = "dd.mm.yyyy hh:mm:ss";
            ws.Column(2).AutoFit();
            ws.Row(1).Style.Font.Bold = true;
        }
    }
}
