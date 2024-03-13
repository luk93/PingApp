using PingApp.Stores;
using PingApp.ViewModels.Base;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PingApp.ViewModels
{
    public class StatusBarViewModel: ViewModelBase
    {
        private readonly LoggsStore _loggsStore;
        public string? LastLogItem => _loggsStore.LastLogItem;
        private readonly StatusStore _statusStore;
        public string? Status => _statusStore?.Status;
        public int? MaxProgress => _statusStore?.MaxProgress;
        public int? ActProgress => _statusStore?.ActProgress;
        public StatusBarViewModel(LoggsStore loggsStore, StatusStore statusStore)
        {
            _loggsStore = loggsStore;
            _statusStore = statusStore;
            _loggsStore.LogItems.CollectionChanged += LogItems_CollectionChanged;
            _statusStore.StatusChanged += StatusStore_StateChanged;
        }

        private void StatusStore_StateChanged()
        {
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(MaxProgress));
            OnPropertyChanged(nameof(ActProgress));
        }

        private void LogItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                if (sender is ObservableCollection<LogEvent> collection)
                    if (collection.Count > 0)
                        OnPropertyChanged(nameof(LastLogItem));
        }
        public override void Dispose()
        {
            _loggsStore.LogItems.CollectionChanged -= LogItems_CollectionChanged;
            _statusStore.StatusChanged -= StatusStore_StateChanged;
            base.Dispose();
        }
    }
}
