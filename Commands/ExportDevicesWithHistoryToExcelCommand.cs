using AutoMapper;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PingApp.DbServices;
using PingApp.Models;
using PingApp.Services;
using PingApp.Stores;
using PingApp.Tools;
using PingApp.ViewModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using static PingApp.Models.Device;

namespace PingApp.Commands
{
    public class ExportDevicesWithHistoryToExcelCommand : ExportDevicesToExcelCommand
    {
        public ExportDevicesWithHistoryToExcelCommand(DeviceListStore deviceStore, StatusStore statusStore, IMapper mapper) : base(deviceStore, statusStore, mapper)
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
                var ws = excelPackage.Workbook.Worksheets.Add($"{device.Name}_pingHistory");
                var range = ws.Cells["A1"].LoadFromCollection(_mapper.Map<List<PingResultExport>>(device.PingResults), true);
                range.AutoFitColumns();
                StyleHistoryWorksheet(ws,range);
            }
            catch(Exception ex) 
            {
                var msg = $"Error message: {ex.Message}, Stack: {ex.StackTrace}";
                Log.Error(msg);
            }
        }
        protected static void StyleHistoryWorksheet(ExcelWorksheet ws, ExcelRangeBase range)
        {
            ws.Column(1).AutoFit();
            ws.Column(2).Style.Numberformat.Format = "dd.mm.yyyy hh:mm:ss";
            ws.Column(2).AutoFit();

            var border = range.Style.Border;
            border.Top.Style = ExcelBorderStyle.Thin;
            border.Left.Style = ExcelBorderStyle.Thin;
            border.Right.Style = ExcelBorderStyle.Thin;
            border.Bottom.Style = ExcelBorderStyle.Thin;

            var firstRowFillStyle = ws.Cells[1, 1, 1, range.Columns].Style.Fill;
            firstRowFillStyle.SetBackground(Color.LightGray);

            for (int i = 2; i <= range.Rows; i++)
            {
                var rowFillStyle = ws.Cells[i, 1, i, range.Columns].Style.Fill;
                switch (ws.Cells[i, 1].Value)
                {
                    case nameof(IPStatus.Success):
                        rowFillStyle.SetBackground(Color.LightGreen);
                        break;
                    case nameof(IPStatus.Unknown):
                        rowFillStyle.SetBackground(Color.Transparent);
                        break;
                    default:
                        rowFillStyle.SetBackground(Color.LightCoral);
                        break;
                }
            }

            ws.Row(1).Style.Font.Bold = true;
        }
    }
}
