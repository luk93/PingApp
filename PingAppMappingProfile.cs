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
            CreateMap<DeviceDTO, Device>();
            CreateMap<Device, DeviceDTO>();
        }
    }
}
