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
    public class GetDevicesFromExcelCommand(DeviceListStore deviceStore, ILogger logger, DeviceListService deviceListService, DeviceDbService deviceDbService) : AsyncCommandBase
    {
        private readonly DeviceListService _deviceListService = deviceListService;
        private readonly DeviceListStore _deviceStore = deviceStore;
        private readonly ILogger _logger = logger;
        private FileInfo? _xlsxFile = null; 
        private readonly DeviceDbService _deviceDbService = deviceDbService;

        public override async Task ExecuteAsync(object? parameter)
        {
            _xlsxFile = FileTools.SelectXlsxFileAndTryToUse("Select excel file which contains Devices (Name,IP Address) (.xlsx)");
            if (_xlsxFile == null) return;
            Log.Information($"File '{_xlsxFile.FullName}' selected!");
            var deviceList = await _deviceListService.UpdateDevicesFromExcelFile(_xlsxFile);
            if (deviceList == null) return;
            _deviceStore.Load(deviceList);
            try
            {
                await _deviceDbService.DeleteAll();
                foreach (var device in _deviceStore.DeviceList)
                {
                    await _deviceDbService.Create(device);
                }
            }
            catch (Exception ex)
            {
                var msg = $"Error message: {ex.Message}, Stack: {ex.StackTrace}";
                _logger.Error(msg);
                Log.Error(msg);
            }
        }
        
    }
}
