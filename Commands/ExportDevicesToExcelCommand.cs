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
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PingApp.Models.Device;

namespace PingApp.Commands
{
    public class ExportDevicesToExcelCommand : AsyncCommandBase
    {
        protected readonly DeviceListStore _deviceStore;
        protected readonly StatusStore _statusStore;
        protected readonly ILogger _logger;
        protected readonly IMapper _mapper;

        public ExportDevicesToExcelCommand(DeviceListStore deviceStore, StatusStore statusStore, ILogger logger, IMapper mapper)
        {
            _deviceStore = deviceStore;
            _statusStore = statusStore;
            _logger = logger;
            _mapper = mapper;

            _deviceStore.Loaded += DeviceStore_Loaded;
            _statusStore.StatusChanged += StatusStore_StatusChanged;
        }

        private void StatusStore_StatusChanged()
        {
            OnCanExecutedChanged();
        }

        private void DeviceStore_Loaded(List<DeviceDTO> deviceList)
        {
            OnCanExecutedChanged();
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            if (!IsXlsxExportPathSelected())
                return;
            var xlsxFilePath = $"{_deviceStore.XlsxExportPath}\\DevicesPingStatus_{FileTools.GetDateTimeString()}.xlsx";
            var excelPackage = CreateExcelPackage(xlsxFilePath);
            if (excelPackage == null)
                return;
            try
            {
                CreateAndStyleDeviceListSheet(excelPackage);
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
        protected void CreateAndStyleDeviceListSheet(ExcelPackage excelPackage)
        {
            var ws = excelPackage.Workbook.Worksheets.Add("DeviceList");
            var range = ws.Cells["A1"].LoadFromCollection(_mapper.Map<List<DeviceExport>>(_deviceStore.DeviceList), true);
            range.AutoFitColumns();
            StyleDeviceListWorksheet(ws, range);
        }
        protected ExcelPackage? CreateExcelPackage(string xlsxFilePath) 
        {
            var excelPackage = ExcelManager.CreateExcelFile(xlsxFilePath);
            if (excelPackage == null)
            {
                var msg = $"Failed to create (.xlsx) file '{xlsxFilePath}'!";
                Log.Error(msg);
                _logger.Error(msg);
                return null;
            }
            return excelPackage;
        }
        protected bool IsXlsxExportPathSelected()
        {
            if (!Directory.Exists(_deviceStore.XlsxExportPath))
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
                else
                {
                    Log.Warning("Export path not selected!");
                    return false;
                }
            }
            return true;
        }
        protected static void StyleDeviceListWorksheet(ExcelWorksheet ws, ExcelRangeBase range)
        {
            ws.Column(5).Style.Numberformat.Format = "dd.mm.yyyy hh:mm:ss";
            ws.Column(5).AutoFit();
            ws.Column(6).Style.Numberformat.Format = "dd.mm.yyyy hh:mm:ss";
            ws.Column(6).AutoFit();

            var border = range.Style.Border;
            border.Top.Style = ExcelBorderStyle.Thin;
            border.Left.Style = ExcelBorderStyle.Thin;
            border.Right.Style = ExcelBorderStyle.Thin;
            border.Bottom.Style = ExcelBorderStyle.Thin;

            var firstRowFillStyle = ws.Cells[1,1,1,range.Columns].Style.Fill;
            firstRowFillStyle.SetBackground(Color.LightGray);

            for (int i = 2; i <= range.Rows; i++) 
            {
                var rowFillStyle = ws.Cells[i,1,i,range.Columns].Style.Fill;
                switch (ws.Cells[i,3].Value)
                {
                    case nameof(PingStatus.Success):
                        rowFillStyle.SetBackground(Color.LightGreen);
                        break;
                    case nameof(PingStatus.None):
                        rowFillStyle.SetBackground(Color.Transparent);
                        break;
                    default:
                        rowFillStyle.SetBackground(Color.LightCoral);
                        break;
                }
            }

            ws.Row(1).Style.Font.Bold = true;

        }
        public override bool CanExecute(object? parameter)
        {
            return _deviceStore.DeviceList.Count > 0 && !_statusStore.IsAppBusy && base.CanExecute(parameter);
        }
    }
}
