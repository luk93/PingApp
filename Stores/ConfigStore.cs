using PingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Stores
{
    public class ConfigStore
    {
        private readonly ObservableCollection<Config>? _config;
        public ObservableCollection<Config>? Configs => _config;
        public Config? SelectedConfig => _config?.FirstOrDefault() ?? null;

        public void UpdateSelectedConfig(Config? selectedConfig)
        {
            var config = _config?.FirstOrDefault(c => c.Id == selectedConfig.Id);
            config = selectedConfig;
        }
        public ConfigStore()
        {
            _config = [];
            _config.Add(new()
            {
                Id = 0,
                StartRow = 2,
                StartColumn = 1,
                SheetIndex = 0,
                PingerData = "################################",
                PingerTimeout = 3000,
                PingerRepeatCount = 1,
            });
        }
    }
}
