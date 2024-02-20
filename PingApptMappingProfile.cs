using AutoMapper;
using PingApp.Models;
using System.IO;
using System.Net.Sockets;

namespace Restaurant_API
{
    public class PingApptMappingProfile : Profile
    {
        public PingApptMappingProfile()
        {
            CreateMap<Device, DeviceDb>();
            CreateMap<DeviceDb, Device>();
        }


    }
}
