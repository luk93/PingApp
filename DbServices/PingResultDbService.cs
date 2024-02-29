using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PingApp.Db;
using PingApp.DbServices.Common;
using PingApp.Models;
using PingApp.ViewModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.DbServices
{
    public class PingResultDbService(AppDbContextFactory contextFactory, IMapper mapper, ILogger logger)
    {
        private readonly AppDbContextFactory _contextFactory = contextFactory;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger _logger = logger;
        private readonly NonQueryDataService<PingResult> _nonQueryDataService = new(contextFactory);

        public async Task<PingResult?> Create(int deviceId, PingResult pingResult)
        {
            using var context = _contextFactory.CreateDbContext();
            pingResult.DeviceId ??= deviceId;
            await context.PingResults.AddAsync(pingResult);
            await context.SaveChangesAsync();
            return pingResult;
        }
        public async Task<bool> Delete(int deviceId, int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var device = GetDeviceById(deviceId);
            if (device == null) return false;
            var pingResult = context.PingResults.FirstOrDefault(p => p.Id == id);
            if (pingResult == null || pingResult.DeviceId != deviceId)
            {
                _logger.Error($"Ping Result with id {deviceId} was not found!");
                return false;
            }
            context.PingResults.Remove(pingResult);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAll(int deviceId)
        {
            using var context = _contextFactory.CreateDbContext();
            var device = GetDeviceById(deviceId);
            if (device == null) return false; 
            context.PingResults.RemoveRange(device.PingResults);
            await context.SaveChangesAsync();
            return true;
        }
        public PingResult? Get(int deviceId, int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var device = GetDeviceById(deviceId);
            if (device == null) return null;
            var pingResult = context.PingResults.FirstOrDefault(p => p.Id == id);
            if (pingResult == null || pingResult.DeviceId != deviceId)
            {
                _logger.Error($"Ping Result with id {deviceId} was not found!");
                return null;
            }
            return pingResult;
        }
        public List<PingResult>? GetAll(int deviceId)
        {
            using var context = _contextFactory.CreateDbContext();
            var device = GetDeviceById(deviceId);
            if (device == null) return null;
            return device.PingResults;
        }
        public async Task<PingResult?> Update(int id, PingResult pingResult) => await _nonQueryDataService.Update(id, pingResult);
        
        private Device? GetDeviceById(int deviceId)
        {
            using var context = _contextFactory.CreateDbContext();
            var device = context.Devices
                                .Include(d => d.PingResults)
                                .FirstOrDefault(d => d.Id == deviceId);
            if (device == null)
            {
                _logger.Error($"Device with id {deviceId} was not found!");
                return null;
            }
            return device;
        }

    }
}
