using OfficeOpenXml;
using PingApp.Models;
using PingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static PingApp.Models.Device;

namespace PingApp.Services
{
    class DeviceListService
    {
        private ObservableCollection<Device> _deviceList;
        public DeviceListService(ObservableCollection<Device> deviceList)
        {
            _deviceList = deviceList;
        }
        public async Task UpdateDevicesFromExcelFile(FileInfo file)
        {
            _deviceList.Clear();
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
                        _deviceList.Add(newObj);
                    }
                    row++;
                }
            }
        }
    }
}
