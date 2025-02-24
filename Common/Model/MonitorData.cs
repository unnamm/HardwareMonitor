using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public partial class MonitorData : ObservableObject
    {
        [ObservableProperty] private string _name = string.Empty;
        [ObservableProperty] private string _sensorType = string.Empty;
        [ObservableProperty] private string _hardwareType = string.Empty;
        [ObservableProperty] private double _value;
        [ObservableProperty] private string _amount = string.Empty;
    }
}
