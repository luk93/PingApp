using AutoMapper;
using PingApp.Models;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace PingApp
{
    public class PingAppMappingProfile : Profile
    {
        public PingAppMappingProfile()
        {
            CreateMap<DeviceDTO, Device>();
            CreateMap<Device, DeviceDTO>();

            CreateMap<DeviceDTO, DeviceExport>()
                .ForMember(dest => dest.LastSuccessfulReplyDt, opt =>
                opt.MapFrom(src =>
                src.PingResults
                .Where(x => x.IpStatus == IPStatus.Success)
                .OrderByDescending(x => x.ReplyDt)
                .Select(x => x.ReplyDt)
                .FirstOrDefault()));
            CreateMap<DeviceExport, DeviceDTO>();

            CreateMap<PingResultExport, PingResult>();
            CreateMap<PingResult, PingResultExport>();
        }
    }
}
