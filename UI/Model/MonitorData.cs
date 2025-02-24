using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Model
{
    partial class MonitorData : ObservableObject
    {
        [ObservableProperty] private string _name = string.Empty;
        [ObservableProperty] private double _value;
    }
}
