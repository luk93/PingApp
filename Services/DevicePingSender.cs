using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PingApp.DbServices;
using PingApp.Extensions;
using PingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interop;

namespace PingApp.Tools
{
    class DevicePingSender
    {
        private readonly Ping _ping;
        private readonly ILogger _logger;
        private readonly string _data;
        private readonly byte[] _buffer;
        private readonly int _timeout;
        private readonly PingOptions _options;
        private readonly List<Device> _deviceList;
        private readonly Queue<Device> _deviceQueue;
        private bool _isSending;
        private readonly TextBlock _tbStatus;
        private readonly DeviceRecordService _deviceRecordService;

        public event EventHandler<EventArgs> DeviceChanged;
        public DevicePingSender(List<Device> deviceList, string data, int timeout, ILogger logger, TextBlock tbStatus, DeviceRecordService deviceRecordService)
        {
            _deviceList = deviceList;
            _ping = new Ping();
            _logger = logger;
            _tbStatus = tbStatus;
            _deviceRecordService = deviceRecordService;
            _data = data;
            _buffer = Encoding.ASCII.GetBytes(_data);
            _timeout = timeout;
            _options = new PingOptions(64, true);
            _deviceQueue = new Queue<Device>();
            _isSending = false;


            _ping.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);
        }
        private void OnDeviceChange(object sender) 
        {
            DeviceChanged?.Invoke(sender, EventArgs.Empty);
        }
        public void SendPingToDeviceList()
        {
            foreach (Device device in _deviceList)
            {
                _deviceQueue.Enqueue(device);
            }
            SendPingToNextDevice();
        }
        private void SendPingToNextDevice()
        {
            if (!_isSending && _deviceQueue.Count > 0)
            {
                _isSending = true;
                Device nextDevice = _deviceQueue.Dequeue();
                nextDevice.IsBusy = true;
                OnDeviceChange(nextDevice);
                _ping.SendAsync(nextDevice.IpAddress, _timeout, _buffer, _options, nextDevice);
            }
        }
        public async void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            Device feedbackDevice = (Device)e.UserState;
            if (e.Cancelled) PingCancelled(e, feedbackDevice);
            else if (e.Error != null) PingError(e, feedbackDevice);
            else PingFeedback(e, feedbackDevice);
            feedbackDevice.IsBusy = false;
            var entity = await _deviceRecordService.GetByIpAddressAndName(feedbackDevice.IpAddress, feedbackDevice.Name);
            if (entity != null) 
            {
                await _deviceRecordService.Update(entity.Id, feedbackDevice);
            }
            OnDeviceChange(feedbackDevice);
            _isSending = false;
            SendPingToNextDevice();
        }
        private void PingCancelled(PingCompletedEventArgs e, Device feedbackDevice)
        {
            feedbackDevice.LastReply = e.Reply;
            feedbackDevice.LastReplyDt = DateTime.Now;
            feedbackDevice.Status = Device.PingStatus.Canceled;
            feedbackDevice.IpStatus = e.Reply.Status;
            OnDeviceChange(feedbackDevice);
            var msg = $"{feedbackDevice.IpAddress}: Ping canceled!";
            _logger.LogWarning(msg);
            _tbStatus.AddLine($"{DateTime.Now} - {msg}");
        }
        private void PingError(PingCompletedEventArgs e, Device feedbackDevice)
        {
            feedbackDevice.LastReply = e.Reply;
            feedbackDevice.LastReplyDt = DateTime.Now;
            feedbackDevice.Status = Device.PingStatus.Failure;
            feedbackDevice.IpStatus = e.Reply.Status;
            OnDeviceChange(feedbackDevice);
            var msg = $"{feedbackDevice.IpAddress}: Ping failed: {e.Error}";
            _logger.LogWarning(msg);
            _tbStatus.AddLine($"{DateTime.Now} - {msg}");
        }
        private void PingFeedback(PingCompletedEventArgs e, Device feedbackDevice)
        {
            if (e.Reply == null)
            {
                feedbackDevice.LastReply = e.Reply;
                feedbackDevice.LastReplyDt = DateTime.Now;
                feedbackDevice.Status = Device.PingStatus.None;
                OnDeviceChange(feedbackDevice);
                return;
            }

            if (e.Reply.Status == IPStatus.Success)
            {
                feedbackDevice.LastReply = e.Reply;
                feedbackDevice.LastReplyDt = DateTime.Now;
                feedbackDevice.Status = Device.PingStatus.Success;
                feedbackDevice.IpStatus = e.Reply.Status;
                OnDeviceChange(feedbackDevice);
                var msg = $"{feedbackDevice.IpAddress}: Ping correct! RoundTrip time: {e.Reply.RoundtripTime}, Time to live: {e.Reply.Options.Ttl}, Size: {e.Reply.Buffer.Length}";
                _logger.LogInformation(msg);
                _tbStatus.AddLine($"{DateTime.Now} - {msg}");
            }
            else
            {
                feedbackDevice.LastReply = e.Reply;
                feedbackDevice.LastReplyDt = DateTime.Now;
                feedbackDevice.Status = Device.PingStatus.Failure;
                feedbackDevice.IpStatus = e.Reply.Status;
                OnDeviceChange(feedbackDevice);
                var msg = $"{feedbackDevice.IpAddress}: Ping failed! {e.Reply.Status}";
                _logger.LogWarning(msg);
                _tbStatus.AddLine($"{DateTime.Now} - {msg}");
            }
        }

    }
}
