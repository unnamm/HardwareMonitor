using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ViewModel
{
    class MainWindowViewModel
    {
        public ObservableCollection<string> Test { get; } = [];

        public MainWindowViewModel()
        {
            Test.Add("1");
            Test.Add("2");
            Test.Add("3");
        }
    }
}
