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

namespace PingApp.Commands
{
    public class ExportDevicesWithoutHistoryToExcelCommand : ExportDevicesToExcelCommand
    {
        public ExportDevicesWithoutHistoryToExcelCommand(DeviceListStore deviceStore, StatusStore statusStore, IMapper mapper) : base(deviceStore, statusStore, mapper)
        {
        }
    }
}
