using PingApp.Models;
using PingApp.State.Navigators;
using PingApp.Stores;
using PingApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static PingApp.ViewModels.Base.ViewModelBase;

namespace PingApp.ViewModels.Factories
{
    public class PingAppViewModelFactory : IPingAppViewModelFactory<DeviceListViewModel>
    {
        private readonly DeviceListStore _deviceStore;
        public PingAppViewModelFactory(DeviceListStore deviceStore)
        {
            _deviceStore = deviceStore;
        }

        public DeviceListViewModel CreateViewModel()
        {
            return new DeviceListViewModel(_deviceStore);
        }
    }
}
