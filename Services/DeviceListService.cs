using OfficeOpenXml;
using PingApp.Models;
using PingApp.Stores;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using static PingApp.Models.DeviceDTO;

namespace PingApp.Services
{
    public class DeviceListService(DeviceListStore deviceList, ConfigStore configStore)
    {
        private readonly ConfigStore _configStore = configStore;
        private readonly DeviceListStore _deviceStore = deviceList;

        public async Task<List<DeviceDTO>?> UpdateDevicesFromExcelFile(FileInfo file)
        {
            _deviceStore.DeviceList.Clear();
            var package = new ExcelPackage(file);
            await package.LoadAsync(file);
            int wsIndex = (int)_configStore.SelectedConfig.SheetIndex;
            if (package.Workbook.Worksheets.Count < wsIndex + 1)
            {
                Log.Error($"Worksheet with index '{wsIndex}' does not exist! Change import excel configuration");
                return null;
            }
            var ws = package.Workbook.Worksheets[wsIndex];
            int row = (int)_configStore.SelectedConfig.StartRow;
            int col = (int)_configStore.SelectedConfig.StartColumn;
            int invalidCount = 0;
            if (ws != null)
            {
                while (!string.IsNullOrWhiteSpace(ws.Cells[row, col].Value?.ToString()))
                {
                    if (!string.IsNullOrWhiteSpace(ws.Cells[row, col + 1].Value?.ToString()))
                    {
                        var name = (ws.Cells[row, col].Value.ToString());
                        if (!IsDeviceNameValid(name))
                        {
                            invalidCount++;
                            Log.Error($"Error while trying to get 'Name' of device from excel file. Row:{row} Column:{col}");
                            row++;
                            continue;
                        }
                        var ipString = (ws.Cells[row, col + 1].Value.ToString());
                        if (!IsIpAddressValid(ipString))
                        {
                            Log.Error($"Error while trying to get 'Ip Address' of device from excel file. Row:{row} Column:{col}");
                            invalidCount++;
                            row++;
                            continue;
                        }
                        DeviceDTO newObj = new(name, ipString);
                        _deviceStore.DeviceList.Add(newObj);
                    }
                    row++;
                }
            }
            if (invalidCount > 0) Log.Warning($"Imported {_deviceStore.DeviceList.Count} devices! There were some invalid ones, check loggs for details!");
            else Log.Information($"Imported {_deviceStore.DeviceList.Count} devices!");
            return _deviceStore.DeviceList;
        }

        internal DeviceListStore GetDeviceStore()
        {
            return _deviceStore;
        }
        private static bool IsDeviceNameValid(string? deviceName)
        {
            if(string.IsNullOrWhiteSpace(deviceName)) return false;
            return true;
        }
        private static bool IsIpAddressValid(string? ipAddress)
        {
            if(string.IsNullOrWhiteSpace(ipAddress)) return false;
            if(Tools.Converters.ConvertStrToIpAddress(ipAddress) == null) return false;
            return true;
        }

    }
}
