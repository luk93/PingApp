using OfficeOpenXml;
using PingApp.Models;
using PingApp.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using static PingApp.Models.Device;

namespace PingApp.Services
{
    public class DeviceListService(DeviceListStore deviceList)
    {
        private readonly DeviceListStore _deviceStore = deviceList;

        public async Task<List<Device>> UpdateDevicesFromExcelFile(FileInfo file)
        {
            _deviceStore.DeviceList.Clear();
            var package = new ExcelPackage(file);
            await package.LoadAsync(file);
            var ws = package.Workbook.Worksheets[0];
            int row = 2;
            int col = 1;
            if (ws != null)
            {
                while (!string.IsNullOrWhiteSpace(ws.Cells[row, col].Value?.ToString()))
                {
                    if (!string.IsNullOrWhiteSpace(ws.Cells[row, col + 1].Value?.ToString()))
                    {
                        var name = (ws.Cells[row, col].Value.ToString());
                        var ipString = (ws.Cells[row, col + 1].Value.ToString());
                        Device newObj = new(name, ipString);
                        _deviceStore.DeviceList.Add(newObj);
                    }
                    row++;
                }
            }
            return _deviceStore.DeviceList;
        }

        internal DeviceListStore GetDeviceStore()
        {
            return _deviceStore;
        }
    }
}
