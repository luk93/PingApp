using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class DeviceRecordService(AppDbContextFactory contextFactory, IMapper mapper)
    {
        private readonly AppDbContextFactory _contextFactory = contextFactory;
        private readonly IMapper _mapper = mapper;
        private readonly NonQueryDataService<DeviceDb> _nonQueryDataService = new(contextFactory);

        public async Task<Device?> Create(Device device) => _mapper.Map<Device>(await _nonQueryDataService.Create(_mapper.Map<DeviceDb>(device)));
        public async Task<bool> Delete(int id) => await _nonQueryDataService.Delete(id);
        public async Task<bool> DeleteAll() => await _nonQueryDataService.DeleteAll();
        public async Task<Device?> Get(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            return _mapper.Map<Device>(await context.Set<DeviceDb>().FirstOrDefaultAsync((e) => e.Id == id) ?? null);
        }
        public async Task<Device?> GetByIpAddressAndName(IPAddress ipAddress, string name)
        {
            using var context = _contextFactory.CreateDbContext();
            return _mapper.Map<Device>(await context.Set<DeviceDb>().FirstOrDefaultAsync((e) => e.IpAddress == ipAddress && e.Name == name) ?? null);
        }
        public async Task<IEnumerable<Device>?> GetAll()
        {
            using var context = _contextFactory.CreateDbContext();
            return _mapper.Map<IEnumerable<Device>>(await context.Set<DeviceDb>().ToListAsync() ?? null);
        }

        public async Task<Device?> Update(int id, Device device) => _mapper.Map<Device>(await _nonQueryDataService.Update(id, _mapper.Map<DeviceDb>(device)));

    }
}
