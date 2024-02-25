using PingApp.Commands;
using PingApp.Models;
using PingApp.States.Navigators;
using PingApp.Stores;
using PingApp.ViewModels.Base;
using PingApp.ViewModels.Factories;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PingApp.ViewModels
{
    public class ConfigViewModel: ViewModelBase
    {
        private readonly IPingAppViewModelFactory _viewModelFactory;
        private readonly IPingAppNavigator _navigator;
        public ICommand? UpdateCurrentViewModel { get; }
        private ConfigStore _configStore;
        private Config _selectedConfig => _configStore.SelectedConfig;
        public Config SelectedConfig
        {
            get =>_selectedConfig;
            set
            {
                _configStore.UpdateSelectedConfig(_selectedConfig);
                OnPropertyChanged(nameof(SelectedConfig));
            }
        }

        public ConfigViewModel(ConfigStore configStore, IPingAppViewModelFactory viewModelFactory, IPingAppNavigator navigator)
        {
            _configStore = configStore;
            _viewModelFactory = viewModelFactory;
            _navigator = navigator;
            UpdateCurrentViewModel = new UpdateCurrentViewModelCommand(_navigator, _viewModelFactory);
            
        }
    }
}
