using PingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.ViewModels
{
    internal interface IObserver
    {
        void HandleDeviceChanged(Object sender, EventArgs args);
    }
}
