using PingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Stores
{
    public class DeviceListStore
    {
        private List<DeviceDTO> _deviceList;
        private string _xlsxExportPath;
        public string XlsxExportPath
        {
            get 
            { 
                return _xlsxExportPath; 
            }
            set 
            { 
                _xlsxExportPath = value;
            }
        }
        public List<DeviceDTO> DeviceList => _deviceList;
        public event Action<List<DeviceDTO>> Loaded;
        public event Action<List<DeviceDTO>> Updated;
        public DeviceListStore()
        {
            _deviceList = [];
        }
        public void Load(List<DeviceDTO> deviceList)
        {
            _deviceList = deviceList;
            OnLoad(deviceList);
        }
        private void OnLoad(List<DeviceDTO> deviceList)
        {
            Loaded?.Invoke(deviceList);
        }
        public void Update(List<DeviceDTO> deviceList)
        {
            _deviceList = deviceList;
            OnUpdate(deviceList);
        }
        private void OnUpdate(List<DeviceDTO> deviceList)
        {
            Updated?.Invoke(deviceList);
        }
    }
}
