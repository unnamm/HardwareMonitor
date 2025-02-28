using Common.Message;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI.View
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isDispose;

        public MainWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource("MaterialDesignWindow");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_isDispose == false)
            {
                e.Cancel = true;
                EndProcess();
            }
            base.OnClosing(e);
        }

        private async void EndProcess()
        {
            await Task.Delay(1);
            _isDispose = true;

            WeakReferenceMessenger.Default.Send(new ExitMessage());

            base.Close();
        }
    }
}
