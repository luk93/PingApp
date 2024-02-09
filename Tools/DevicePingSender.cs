using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        private readonly DeviceList _deviceList;
        private readonly Queue<Device> _deviceQueue;
        private bool _isSending;

        public DevicePingSender(DeviceList deviceList, string data, int timeout, ILogger logger)
        {
            _deviceList = deviceList;
            _ping = new Ping();
            _logger = logger;
            _data = data;
            _buffer = Encoding.ASCII.GetBytes(_data);
            _timeout = timeout;
            _options = new PingOptions(64, true);
            _deviceQueue = new Queue<Device>();
            _isSending = false;

            _ping.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);
        }
        public void SendPingToDeviceList()
        {
            foreach (Device device in _deviceList.Devices)
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
                _ping.SendAsync(nextDevice.IpAddress, _timeout, _buffer, _options, nextDevice);
            }
        }
        public void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            Device feedbackDevice = (Device)e.UserState;
            if (e.Cancelled) PingCancelled(e, feedbackDevice);
            else if (e.Error != null) PingError(e, feedbackDevice);
            else PingFeedback(e, feedbackDevice);

            feedbackDevice.IsBusy = false;
            _isSending = false;
            SendPingToNextDevice();
        }
        private void PingCancelled(PingCompletedEventArgs e, Device feedbackDevice)
        {
            feedbackDevice.LastReply = e.Reply;
            feedbackDevice.LastReplyDt = DateTime.Now;
            feedbackDevice.Status = Device.PingStatus.Canceled;
            feedbackDevice.IPStatus = e.Reply.Status;
            _logger.LogWarning($"{feedbackDevice.IpAddress}: Ping canceled!");
        }
        private void PingError(PingCompletedEventArgs e, Device feedbackDevice)
        {
            feedbackDevice.LastReply = e.Reply;
            feedbackDevice.LastReplyDt = DateTime.Now;
            feedbackDevice.Status = Device.PingStatus.Failure;
            feedbackDevice.IPStatus = e.Reply.Status;
            _logger.LogWarning($"{feedbackDevice.IpAddress}: Ping failed: {e.Error}");
        }
        private void PingFeedback(PingCompletedEventArgs e, Device feedbackDevice)
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
                feedbackDevice.IPStatus = e.Reply.Status;
                _logger.LogInformation($"{feedbackDevice.IpAddress}: Ping correct ! RoundTrip time: {e.Reply.RoundtripTime}, Time to live: {e.Reply.Options.Ttl}, Size: {e.Reply.Buffer.Length}");
            }
            else
            {
                feedbackDevice.LastReply = e.Reply;
                feedbackDevice.LastReplyDt = DateTime.Now;
                feedbackDevice.Status = Device.PingStatus.Failure;
                feedbackDevice.IPStatus = e.Reply.Status;
                _logger.LogWarning($"{feedbackDevice.IpAddress}: Ping failed ! {e.Reply.Status}");
            }
        }

    }
}
