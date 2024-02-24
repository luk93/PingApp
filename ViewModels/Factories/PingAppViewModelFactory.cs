using PingApp.States.Navigators;
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
        private readonly CreateViewModel<ConfigViewModel> _createConfigViewModel;

        public PingAppViewModelFactory(CreateViewModel<DeviceListViewModel> createDeviceListViewModel, 
            CreateViewModel<ConfigViewModel> createConfigViewModel)
        {
            _createDeviceListViewModel = createDeviceListViewModel;
            _createConfigViewModel = createConfigViewModel;
        }

        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.DeviceList:
                    return _createDeviceListViewModel();
                case ViewType.Config:
                    return _createConfigViewModel();
                default:
                    throw new ArgumentException("The ViewType does not hava a ViewModel", "viewType");
            }
        }
    }
}
