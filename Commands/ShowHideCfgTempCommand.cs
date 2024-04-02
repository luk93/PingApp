using PingApp.Stores;
using PingApp.Tools;
using PingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Commands
{
    public class ShowHideCfgTempCommand(ConfigStore configStore) : AsyncCommandBase
    {
        private readonly ConfigStore _configStore = configStore;

        public override Task ExecuteAsync(object? parameter)
        {
            _configStore.SelectedConfig.IsExcelTemplateShown = !_configStore.SelectedConfig.IsExcelTemplateShown;
            return Task.CompletedTask;
        }   
    }
}
