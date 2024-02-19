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
using static PingApp.ViewModels.Base.ViewModelBase;

namespace PingApp.ViewModels.Factories
{
    public class PingAppViewModelFactory : IPingAppViewModelFactory
    {
        private readonly CreateViewModel<DeviceListViewModel> _createDeviceListViewModel;

        public PingAppViewModelFactory(CreateViewModel<DeviceListViewModel> createDeviceListViewModel)
        {
            _createDeviceListViewModel = createDeviceListViewModel;
        }

        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.DeviceList:
                    return _createDeviceListViewModel();
                default:
                    throw new ArgumentException("The ViewType does not hava a ViewModel", "viewType");
            }
        }
    }
}
