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
        private string _data;
        private byte[] _buffer;
        private int _timeout;
        private readonly PingOptions _options;
        private readonly DeviceListService _deviceListService;
        private readonly Queue<DeviceDTO> _deviceQueue;
        private readonly DeviceDbService _deviceDbService;
        private readonly PingResultDbService _pingResultDbService;
        private readonly StatusStore _statusStore;
        private readonly ConfigStore _configStore;
        private bool _isBusy;
        private bool _isCanceled;
        private int _pingRepeatCountConfig;
        private int _pingRepeatCount;

        public DevicePingSender(DeviceListService deviceListService, ILogger logger, DeviceDbService deviceDbService,
            StatusStore statusStore, ConfigStore configStore, PingResultDbService pingResultDbService)
        {
            _configStore = configStore;
            _deviceListService = deviceListService;
            _ping = new Ping();
            _logger = logger;
            _deviceDbService = deviceDbService;
            _pingResultDbService = pingResultDbService;
            _data = _configStore.SelectedConfig.PingerData;
            _buffer = Encoding.ASCII.GetBytes(_data);
            _timeout = _configStore.SelectedConfig.PingerTimeout;
            _options = new PingOptions(64, true);
            _deviceQueue = new Queue<DeviceDTO>();
            _isBusy = false;
            _statusStore = statusStore;
            _pingRepeatCountConfig = configStore.SelectedConfig.PingerRepeatCount;

            _ping.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);
        }
        public void SendPingToDeviceList()
        {
            //Update Pinger data
            _isCanceled = false;
            _data = _configStore.SelectedConfig.PingerData;
            _timeout = _configStore.SelectedConfig.PingerTimeout;
            _pingRepeatCountConfig = _configStore.SelectedConfig.PingerRepeatCount;
            _buffer = Encoding.ASCII.GetBytes(_data);

            _statusStore.IsAppBusy = true;
            var devices = _deviceListService.GetDeviceStore().DeviceList;
            _statusStore.Status = "Pinging Devices Ongoing...";
            _statusStore.MaxProgress = devices.Count * (_pingRepeatCountConfig + 1);
            _statusStore.ActProgress = 0;
            foreach (DeviceDTO device in devices)
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
                DeviceDTO? nextDevice;
                if (_pingRepeatCount >= _pingRepeatCountConfig)
                {
                    _pingRepeatCount = 0;
                    nextDevice = _deviceQueue.Dequeue();
                }
                else
                {
                    _pingRepeatCount++;
                    nextDevice = _deviceQueue.Peek();
                }
                nextDevice.Status = Device.PingStatus.Busy;
                if (nextDevice != null && nextDevice.IpAddress != null)
                    _ping.SendAsync(nextDevice.IpAddress, _timeout, _buffer, _options, nextDevice);
            }
            else
            {
                if (_isCanceled)
                {
                    var msg = "Pinging devices canceled!";
                    _statusStore.Status = msg;
                    _statusStore.ActProgress = 0;
                    Log.Information(msg);
                    _logger.Information(msg);
                }
                else
                {
                    var msg = "Pinging devices finished!";
                    _statusStore.Status = msg;
                    Log.Information(msg);
                    _logger.Information(msg);
                }
                _statusStore.IsAppBusy = false;
            }
        }
        public async void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            DeviceDTO? feedbackDevice = (DeviceDTO?)e.UserState ?? null;
            if (feedbackDevice == null) return;
            if (e.Cancelled) PingCancelled(e, feedbackDevice);
            else if (e.Error != null) PingError(e, feedbackDevice);
            else PingFeedback(e, feedbackDevice);
            if (feedbackDevice.IpAddress == null || feedbackDevice.Name == null)
            {
                var msg = "Ping callback came from device with no IpAddress or Name!";
                _logger.Error(msg);
                Log.Error(msg);
            }
            else
            { 
                var device = await _deviceDbService.GetByIpAddressAndName(feedbackDevice.IpAddress, feedbackDevice.Name);
                if (device != null)
                {
                    PingResult pingResult = new()
                    {
                        DeviceId = device.Id,
                        IpStatus = feedbackDevice.LastIpStatus,
                        ReplyDt = feedbackDevice.LastReplyDt,
                    };
                    await _pingResultDbService.Create(device.Id, pingResult);
                    feedbackDevice.PingResults.Insert(0,pingResult);
                    await _deviceDbService.Update(device.Id, feedbackDevice);
                }
            }
            _isBusy = false;
            _statusStore.ActProgress++;
            SendPingToNextDevice();
        }
        private void PingCancelled(PingCompletedEventArgs e, DeviceDTO feedbackDevice)
        {
            feedbackDevice.LastReply = e.Reply;
            feedbackDevice.LastReplyDt = DateTime.Now;
            feedbackDevice.Status = Device.PingStatus.Canceled;
            feedbackDevice.LastIpStatus = e.Reply.Status;
            var msg = $"{feedbackDevice.IpAddress}: Ping canceled!";
            _logger.Warning(msg);
            Log.Warning($"{msg}");
        }
        private void PingError(PingCompletedEventArgs e, DeviceDTO feedbackDevice)
        {
            feedbackDevice.LastReply = e.Reply;
            feedbackDevice.LastReplyDt = DateTime.Now;
            feedbackDevice.Status = Device.PingStatus.Failure;
            feedbackDevice.LastIpStatus = e.Reply.Status;
            var msg = $"{feedbackDevice.IpAddress}: Ping failed: {e.Error}";
            _logger.Warning(msg);
            Log.Warning($"{msg}");
        }
        private void PingFeedback(PingCompletedEventArgs e, DeviceDTO feedbackDevice)
        {
            if (e.Reply == null)
            {
                feedbackDevice.LastReply = e.Reply;
                feedbackDevice.LastReplyDt = DateTime.Now;
                feedbackDevice.Status = Device.PingStatus.None;
                return;
            }

            if (e.Reply.Status == IPStatus.Success)
            {
                feedbackDevice.LastReply = e.Reply;
                feedbackDevice.LastReplyDt = DateTime.Now;
                feedbackDevice.Status = Device.PingStatus.Success;
                feedbackDevice.LastIpStatus = e.Reply.Status;
                var msg = $"{feedbackDevice.IpAddress}: Ping correct! RoundTrip time: {e.Reply.RoundtripTime}, Time to live: {e.Reply?.Options?.Ttl}, Size: {e.Reply?.Buffer.Length}";
                _logger.Information(msg);
                Log.Information($"{msg}");
            }
            else
            {
                feedbackDevice.LastReply = e.Reply;
                feedbackDevice.LastReplyDt = DateTime.Now;
                feedbackDevice.Status = Device.PingStatus.Failure;
                feedbackDevice.LastIpStatus = e.Reply.Status;
                var msg = $"{feedbackDevice.IpAddress}: Ping failed! {e.Reply.Status}";
                _logger.Warning(msg);
                Log.Warning($"{msg}");
            }
        }
        public void CancelPingToDeviceList()
        {
            _isCanceled = true;
            _deviceQueue.Clear();
        }
    }
}
