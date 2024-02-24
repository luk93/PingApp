using PingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Stores
{
    public class StatusStore
    {
        private int? _maxProgress;
        public int? MaxProgress
        {
            get => _maxProgress;
            set
            {
               _maxProgress = value;
                StatusChanged?.Invoke();
            }
        }
        private int? _actProgress;
        public int? ActProgress
        {
            get => _actProgress;
            set
            {
                _actProgress = value;
                StatusChanged?.Invoke();
            }
        }
        public string? _status;
        public string? Status
        {
            get => _status;
            set
            {
                _status = value;
                StatusChanged?.Invoke();
            }
        }
        public bool _isAppBusy;
        public bool IsAppBusy
        {
            get => _isAppBusy;
            set 
            {
                _isAppBusy = value;
                StatusChanged?.Invoke();
            }
        }
        public StatusStore()
        {
            _isAppBusy = false;
            _maxProgress = 100;
            _actProgress = 10;
            _status = "Waiting for operations";
        }
        public event Action? StatusChanged;
    }
}
