using PingApp.State.Navigators;
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
    public class UpdateCurrentViewModelCommand : CommandBase
    {

        private readonly IPingAppNavigator _navigator;
        private readonly IPingAppViewModelFactory _viewModelFactory;

        public UpdateCurrentViewModelCommand(IPingAppNavigator navigator, IPingAppViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        public override void Execute(object? parameter)
        {
           if(parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;
                _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
            }
        }
    }
}