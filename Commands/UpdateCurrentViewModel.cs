using PingApp.States.Navigators;
using PingApp.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PingApp.Commands
{
    public class UpdateCurrentViewModelCommand(IPingAppNavigator navigator, IPingAppViewModelFactory viewModelFactory) : CommandBase
    {

        private readonly IPingAppNavigator _navigator = navigator;
        private readonly IPingAppViewModelFactory _viewModelFactory = viewModelFactory;

        public override void Execute(object? parameter)
        {
            if (parameter is ViewType viewType)
            {
                _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
            }
        }
    }
}