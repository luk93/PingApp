using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PingApp.Db;
using PingApp.DbServices.Common;
using PingApp.Models;
using PingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.DbServices
{
    public class DeviceDbService(AppDbContextFactory contextFactory, IMapper mapper)
    {
        private readonly AppDbContextFactory _contextFactory = contextFactory;
        private readonly IMapper _mapper = mapper;
        private readonly NonQueryDataService<Device> _nonQueryDataService = new(contextFactory);

        public async Task<DeviceDTO?> Create(DeviceDTO device) => _mapper.Map<DeviceDTO>(await _nonQueryDataService.Create(_mapper.Map<Device>(device)));
        public async Task<bool> Delete(int id) => await _nonQueryDataService.Delete(id);
        public async Task<bool> DeleteAll() => await _nonQueryDataService.DeleteAll();
        public async Task<DeviceDTO?> Get(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return _mapper.Map<DeviceDTO>(await context.Devices.FirstOrDefaultAsync((e) => e.Id == id) ?? null);
        }
        public async Task<DeviceDTO?> GetByIpAddressAndName(IPAddress ipAddress, string name)
        {
            using var context = _contextFactory.CreateDbContext();
            return _mapper.Map<DeviceDTO>(await context.Devices.FirstOrDefaultAsync((e) => e.IpAddress == ipAddress && e.Name == name) ?? null);
        }
        public async Task<DeviceDTO?> GetWithPingResults(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return _mapper.Map<DeviceDTO> (await context.Devices
                                                       .Include(f => f.PingResults.OrderByDescending(d => d.ReplyDt))
                                                       .FirstOrDefaultAsync((t) => t.Id == id) ?? null);
        }
        public async Task<IEnumerable<DeviceDTO>?> GetAll()
        {
            using var context = _contextFactory.CreateDbContext();
            return _mapper.Map<IEnumerable<DeviceDTO>>(await context.Devices.ToListAsync() ?? null);
        }
        public async Task<IEnumerable<DeviceDTO>?> GetAllWithPingResults()
        {
            using var context = _contextFactory.CreateDbContext();
            return _mapper.Map<IEnumerable<DeviceDTO>>(await context.Devices
                                                       .Include(f => f.PingResults.OrderByDescending(d => d.ReplyDt))
                                                       .ToListAsync() ?? null);
        }

        public async Task<DeviceDTO?> Update(int id, DeviceDTO device) => _mapper.Map<DeviceDTO>(await _nonQueryDataService.Update(id, _mapper.Map<Device>(device)));

    }
}
