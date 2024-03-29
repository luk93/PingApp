﻿using PingApp.States.Navigators;
using PingApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.ViewModels.Factories
{
    public interface IPingAppViewModelFactory
    {
        ViewModelBase CreateViewModel(ViewType viewType);
    }
}
