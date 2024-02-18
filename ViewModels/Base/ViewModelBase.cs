using PingApp.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.ViewModels.Base
{
    public class ViewModelBase : ObservableObject
    {
        public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase;
        public virtual void Dispose() { }
    }
}
