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
    public class DeviceRecordService
    {
        private readonly AppDbContextFactory _contextFactory;
        private readonly NonQueryDataService<DeviceDb> _nonQueryDataService;
        public DeviceRecordService(AppDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<DeviceDb>(contextFactory);
        }
        public async Task<Device?> Create(Device device)
        {
            DeviceDb deviceDb = new()
            {
                Id = device.Id,
                Name = device.Name,
                Status = device.Status,
                IpStatus = device.IpStatus,
                IpAddress = device.IpAddress,
                IpString = device.IpString,
                LastReplyDt = device.LastReplyDt
            };
            DeviceDb? newDeviceDb = await _nonQueryDataService.Create(deviceDb) ?? null;
            if (newDeviceDb == null) return null;
            Device newDevice = new(deviceDb);
            return newDevice;
        }
        public async Task<bool> Delete(int id)
        {
            return await _nonQueryDataService.Delete(id);
        }
        public async Task<bool> DeleteAll()
        {
            return await _nonQueryDataService.DeleteAll();
        }
        public async Task<Device?> Get(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var deviceDb = await context.Set<DeviceDb>().FirstOrDefaultAsync((e) => e.Id == id) ?? null;
                if (deviceDb == null) return null;
                Device device = new(deviceDb);
                return device;
            }
        }
        public async Task<Device?> GetByIpAddressAndName(IPAddress ipAddress, string name)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var deviceDb = await context.Set<DeviceDb>().FirstOrDefaultAsync((e) => e.IpAddress == ipAddress && e.Name == name) ?? null;
                if (deviceDb == null) return null;
                Device device = new(deviceDb);
                return device;
            }
        }
        public async Task<IEnumerable<Device>?> GetAll()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var devicesDb = await context.Set<DeviceDb>().ToListAsync() ?? null;
                if (devicesDb == null) return null;
                return devicesDb.Select(db => new Device(db));


            }
        }

        public async Task<Device?> Update(int id, Device device)
        {
            DeviceDb deviceDb = new()
            {
                Id = device.Id,
                Name = device.Name,
                Status = device.Status,
                IpStatus = device.IpStatus,
                IpAddress = device.IpAddress,
                IpString = device.IpString,
                LastReplyDt = device.LastReplyDt
            };
            DeviceDb? newDeviceDb = await _nonQueryDataService.Update(id, deviceDb) ?? null;
            if(newDeviceDb == null) return null;
            Device newDevice = new(deviceDb);
            return newDevice;
        }

    }
}
