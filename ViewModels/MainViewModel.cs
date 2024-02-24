using PingApp.States.Navigators;
using PingApp.Controls;
using PingApp.States.Ribbon;
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
        public LoggsViewModel LoggsViewModel { get; set; }
        public StatusBarViewModel StatusBarViewModel { get; set; }
        public IPingAppRibbon Ribbon { get; set; }
        public ViewModelBase? CurrentViewModel => _navigator.CurrentViewModel;
        public ICommand UpdateCurrentViewModelCommand { get; set; }
        public MainViewModel(IPingAppNavigator navigator, LoggsViewModel loggsViewModel, IPingAppRibbon ribbon, 
            IPingAppViewModelFactory viewModelFactory, StatusBarViewModel statusBarViewModel)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
            LoggsViewModel = loggsViewModel;
            Ribbon = ribbon;
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navigator, _viewModelFactory);
            UpdateCurrentViewModelCommand.Execute(ViewType.DeviceList);
            StatusBarViewModel = statusBarViewModel;

            _navigator.StateChanged += Navigator_StateChanged;
        }

        private void Navigator_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        public override void Dispose()
        {
            _navigator.StateChanged -= Navigator_StateChanged;
            base.Dispose();
        }
    }
}