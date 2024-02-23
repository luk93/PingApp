using Microsoft.Extensions.Options;
using PingApp.DbServices;
using PingApp.Extensions;
using PingApp.Models;
using PingApp.Services;
using PingApp.Stores;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interop;

namespace PingApp.Tools
{
    public class DevicePingSender
    {
        private readonly Ping _ping;
        private readonly ILogger _logger;
        private readonly string _data;
        private readonly byte[] _buffer;
        private readonly int _timeout;
        private readonly PingOptions _options;
        private readonly DeviceListService _deviceListService;
        private readonly Queue<Device> _deviceQueue;
        private bool _isBusy;
        private readonly DeviceRecordService _deviceRecordService;
        private readonly StatusStore _statusStore;
        public event EventHandler<EventArgs> DeviceChanged;
        public DevicePingSender(DeviceListService deviceListService, ILogger logger, DeviceRecordService deviceRecordService, StatusStore statusStore)
        {
            _deviceListService = deviceListService;
            _ping = new Ping();
            _logger = logger;
            _deviceRecordService = deviceRecordService;
            _data = "################################";
            _buffer = Encoding.ASCII.GetBytes(_data);
            _timeout = 3000;
            _options = new PingOptions(64, true);
            _deviceQueue = new Queue<Device>();
            _isBusy = false;
            _statusStore = statusStore;


            _ping.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);
        }
        public void SendPingToDeviceList()
        {
            var devices = _deviceListService.GetDeviceStore().DeviceList;
            _statusStore.Status = "Pinging Devices Ongoing...";
            _statusStore.MaxProgress = devices.Count;
            _statusStore.ActProgress = 0;
            foreach (Device device in devices)
            {
                _deviceQueue.Enqueue(device);
            }
            SendPingToNextDevice();
        }
        private void SendPingToNextDevice()
        {
            if (!_isBusy && _deviceQueue.Count > 0)
            {
                _isBusy = true;
                Device nextDevice = _deviceQueue.Dequeue();
                nextDevice.Status = DeviceDb.PingStatus.Busy;
                if (nextDevice != null)
                    _ping.SendAsync(nextDevice.IpAddress, _timeout, _buffer, _options, nextDevice);
            }
        }
        public async void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            Device? feedbackDevice = (Device?)e.UserState ?? null;
            if (feedbackDevice == null) return;
            if (e.Cancelled) PingCancelled(e, feedbackDevice);
            else if (e.Error != null) PingError(e, feedbackDevice);
            else PingFeedback(e, feedbackDevice);
            var entity = await _deviceRecordService.GetByIpAddressAndName(feedbackDevice.IpAddress, feedbackDevice.Name);
            if (entity != null) 
            {
                await _deviceRecordService.Update(entity.Id, feedbackDevice);
            }
            _isBusy = false;
            _statusStore.ActProgress++;
            SendPingToNextDevice();
        }
        private void PingCancelled(PingCompletedEventArgs e, Device feedbackDevice)
        {
            feedbackDevice.LastReply = e.Reply;
            feedbackDevice.LastReplyDt = DateTime.Now;
            feedbackDevice.Status = DeviceDb.PingStatus.Canceled;
            feedbackDevice.IpStatus = e.Reply.Status;
            var msg = $"{feedbackDevice.IpAddress}: Ping canceled!";
            _logger.Warning(msg);
            Log.Warning($"{msg}");
        }
        private void PingError(PingCompletedEventArgs e, Device feedbackDevice)
        {
            feedbackDevice.LastReply = e.Reply;
            feedbackDevice.LastReplyDt = DateTime.Now;
            feedbackDevice.Status = DeviceDb.PingStatus.Failure;
            feedbackDevice.IpStatus = e.Reply.Status;
            var msg = $"{feedbackDevice.IpAddress}: Ping failed: {e.Error}";
            _logger.Warning(msg);
            Log.Warning($"{msg}");
        }
        private void PingFeedback(PingCompletedEventArgs e, Device feedbackDevice)
        {
            if (e.Reply == null)
            {
                feedbackDevice.LastReply = e.Reply;
                feedbackDevice.LastReplyDt = DateTime.Now;
                feedbackDevice.Status = DeviceDb.PingStatus.None;
                return;
            }

            if (e.Reply.Status == IPStatus.Success)
            {
                feedbackDevice.LastReply = e.Reply;
                feedbackDevice.LastReplyDt = DateTime.Now;
                feedbackDevice.Status = DeviceDb.PingStatus.Success;
                feedbackDevice.IpStatus = e.Reply.Status;
                var msg = $"{feedbackDevice.IpAddress}: Ping correct! RoundTrip time: {e.Reply.RoundtripTime}, Time to live: {e.Reply?.Options?.Ttl}, Size: {e.Reply?.Buffer.Length}";
                _logger.Information(msg);
                Log.Information($"{msg}");
            }
            else
            {
                feedbackDevice.LastReply = e.Reply;
                feedbackDevice.LastReplyDt = DateTime.Now;
                feedbackDevice.Status = DeviceDb.PingStatus.Failure;
                feedbackDevice.IpStatus = e.Reply.Status;
                var msg = $"{feedbackDevice.IpAddress}: Ping failed! {e.Reply.Status}";
                _logger.Warning(msg);
                Log.Warning($"{msg}");
            }
        }

    }
}
