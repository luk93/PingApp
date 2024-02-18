using PingApp.State.Navigators;
using PingApp.State.StatusBar;
using PingApp.Controls;
using PingApp.State.Ribbon;
using PingApp.ViewModels.Base;

namespace PingApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public IPingAppNavigator Navigator { get; set; }
        public IPingAppStatusBar StatusBar { get; set; }
        public IPingAppRibbon Ribbon { get; set; }
        public MainViewModel(IPingAppNavigator navigator, IPingAppStatusBar statusBar, IPingAppRibbon ribbon)
        {
            Navigator = navigator;
            Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.DeviceList);
            StatusBar = statusBar;
            Ribbon = ribbon;
        }
    }
}