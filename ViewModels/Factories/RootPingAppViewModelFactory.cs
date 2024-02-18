using PingApp.State.Navigators;
using PingApp.ViewModels.Base;
using PingApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.ViewModels.Factories
{
    public class RootPingAppViewModelFactory : IRootPingAppViewModelFactory
    {
        private readonly IPingAppViewModelFactory<DeviceListViewModel> _deviceListViewModelFactory;

        public RootPingAppViewModelFactory(IPingAppViewModelFactory<DeviceListViewModel> deviceListViewModelFactory)
        {
            _deviceListViewModelFactory = deviceListViewModelFactory;
        }

        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.DeviceList:
                    return _deviceListViewModelFactory.CreateViewModel();
                default:
                    throw new ArgumentException("The ViewType does not hava a ViewModel", "viewType");
            }
        }
    }
}
