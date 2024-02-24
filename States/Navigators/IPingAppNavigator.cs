using PingApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PingApp.States.Navigators
{
    public enum ViewType
    {
        DeviceList,
        Config
    }
    public interface IPingAppNavigator
    {
        
        ViewModelBase? CurrentViewModel { get; set; }
        ICommand UpdateCurrentViewModelCommand { get; }
        event Action? StateChanged;
    }
}
