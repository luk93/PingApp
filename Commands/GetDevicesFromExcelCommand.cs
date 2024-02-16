using PingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Commands
{
    public class GetDevicesFromExcelCommand : AsyncCommandBase
    {
        private List<Device> _deviceList;
        public GetDevicesFromExcelCommand(List<Device> deviceList)
        {
            _deviceList = deviceList;
        }
        public override Task ExecuteAsync(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
