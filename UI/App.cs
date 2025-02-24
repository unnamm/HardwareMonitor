using Common.Config;
using Common.Message;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Function;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UI.View;
using UI.ViewModel;

namespace UI
{
    internal class App : Application
    {
        private readonly IServiceCollection _servicesCollection;
        private readonly Dictionary<Type, Type> _viewPair = [];
        private readonly MainWindow _mainView;
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var builder = Host.CreateApplicationBuilder();
            _servicesCollection = builder.Services;

            #region add
            _servicesCollection.AddSingleton<Flow>();
            _servicesCollection.AddSingleton<MonitorHW>();
            _servicesCollection.AddSingleton<DataConfig>();
            AddViewAndViewModel<MainWindow, MainWindowViewModel>();
            #endregion

            _serviceProvider = builder.Build().Services;
            Ioc.Default.ConfigureServices(_serviceProvider);
            _mainView = _serviceProvider.GetService<MainWindow>()!;

            AutoConnectViewAndViewModel();

            Startup += (x, y) => _mainView.Show(); //mainwindow show

            InitAsync();
        }

        private async void InitAsync()
        {
            await WaitShowWindow();
            _ = _serviceProvider.GetService<Flow>();
            WeakReferenceMessenger.Default.Send(new InitCompleteMessage());
        }

        private async Task WaitShowWindow()
        {
            bool active = false;
            while (true)
            {
                await Task.Delay(1);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    active = _mainView.IsActive;
                });

                if (active == true)
                    break;
            }
        }

        /// <summary>
        /// auto connect view and viewmodel
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void AutoConnectViewAndViewModel()
        {
            foreach (var pair in _viewPair)
            {
                var uc = (ContentControl)Ioc.Default.GetService(pair.Key)!;
                if (uc.DataContext != null)
                {
                    throw new Exception($"{uc} is already allocated DataContext");
                }
                uc.DataContext = Ioc.Default.GetService(pair.Value) ?? throw new Exception("viewmodel null");
            }
        }

        /// <summary>
        /// add view and viewmodel
        /// </summary>
        /// <typeparam name="View"></typeparam>
        /// <typeparam name="ViewModel"></typeparam>
        private void AddViewAndViewModel<View, ViewModel>() where View : ContentControl where ViewModel : class
        {
            _servicesCollection.AddSingleton<View>();
            _servicesCollection.AddSingleton<ViewModel>();

            _viewPair.Add(typeof(View), typeof(ViewModel));
        }
    }
}
