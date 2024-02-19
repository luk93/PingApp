using PingApp.State.Navigators;
using PingApp.State.StatusBar;
using PingApp.Controls;
using PingApp.State.Ribbon;
using PingApp.ViewModels.Base;
using System.Windows.Input;
using PingApp.Commands;
using PingApp.ViewModels.Factories;

namespace PingApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IPingAppNavigator _navigator;
        private readonly IPingAppViewModelFactory _viewModelFactory;
        public IPingAppStatusBar StatusBar { get; set; }
        public IPingAppRibbon Ribbon { get; set; }
        public ViewModelBase? CurrentViewModel => _navigator.CurrentViewModel;
        public ICommand UpdateCurrentViewModelCommand { get; set; }
        public MainViewModel(IPingAppNavigator navigator, IPingAppStatusBar statusBar, IPingAppRibbon ribbon, IPingAppViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
            //Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.DeviceList);
            StatusBar = statusBar;
            Ribbon = ribbon;
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navigator, _viewModelFactory);
            UpdateCurrentViewModelCommand.Execute(ViewType.DeviceList);
            
        }
    }
}