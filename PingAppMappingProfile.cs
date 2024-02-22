using AutoMapper;
using PingApp.Models;
using System.IO;
using System.Net.Sockets;

namespace PingApp
{
    public class PingAppMappingProfile : Profile
    {
        public PingAppMappingProfile()
        {
            CreateMap<Device, DeviceDb>();
            CreateMap<DeviceDb, Device>();
        }
    }
}
