using PingApp.Commands;
using PingApp.ViewModels.Base;
using PingApp.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace PingApp.States.Navigators
{
    public class PingAppNavigator : IPingAppNavigator
    {
        private ViewModelBase? _currentViewModel;
        public ViewModelBase? CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            { 
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                StateChanged?.Invoke();
            }
        }
        public ICommand UpdateCurrentViewModelCommand {get; set;}
        public PingAppNavigator(IPingAppViewModelFactory viewModelFactory)
        {
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(this, viewModelFactory);
        }
        public event Action? StateChanged;
    }
}
