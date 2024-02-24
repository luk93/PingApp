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
        private List<Device> _deviceList;
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
        public List<Device> DeviceList => _deviceList;
        public event Action<List<Device>> Loaded;
        public event Action<List<Device>> Updated;
        public DeviceListStore()
        {
            _deviceList = [];
        }
        public void Load(List<Device> deviceList)
        {
            _deviceList = deviceList;
            OnLoad(deviceList);
        }
        private void OnLoad(List<Device> deviceList)
        {
            Loaded?.Invoke(deviceList);
        }
        public void Update(List<Device> deviceList)
        {
            _deviceList = deviceList;
            OnUpdate(deviceList);
        }
        private void OnUpdate(List<Device> deviceList)
        {
            Updated?.Invoke(deviceList);
        }
    }
}
