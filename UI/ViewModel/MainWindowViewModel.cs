﻿using Common.Message;
using Common.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Function;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UI.ViewModel
{
    class MainWindowViewModel : ObservableRecipient, IRecipient<HardwareInfoMessage>
    {
        public ObservableCollection<MonitorData> Datas { get; } = [];

        public MainWindowViewModel()
        {
            IsActive = true;
        }

        public void Receive(HardwareInfoMessage message)
        {
            if (Application.Current == null)
                return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Datas.Count == 0)
                {
                    foreach (var item in message.Data)
                    {
                        Datas.Add(item);
                    }
                }
                else
                {
                    int i = 0;
                    foreach (var item in message.Data)
                    {
                        Datas[i].Value = item.Value;
                        i++;
                    }
                }
            });
        }

    }
}
