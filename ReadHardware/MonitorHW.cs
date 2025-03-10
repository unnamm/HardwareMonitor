﻿using Common.Config;
using Common.Model;
using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Function
{
    public class MonitorHW
    {
        private readonly int _timeout;
        private readonly Computer _com;

        public MonitorHW(DataConfig config)
        {
            _timeout = config.OpenTimeout;

            _com = new Computer
            {
                //get info hardware
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMotherboardEnabled = true,
                IsStorageEnabled = true,
                IsBatteryEnabled = true,
                IsControllerEnabled = true,
                IsMemoryEnabled = true,
                IsNetworkEnabled = true,
                IsPsuEnabled = true,
            };
        }

        public async Task Open()
        {
            var task = Task.Run(_com.Open);
            var result = await Task.WhenAny(Task.Delay(_timeout), task);

            if (task != result)
            {
                throw new TimeoutException("open fail");
            }
        }

        public async Task<IEnumerable<ISensor>> Read()
        {
            List<ISensor> list = [];

            await Task.Run(() =>
            {
                foreach (var hard in _com.Hardware)
                {
                    hard.Update();
                    foreach (var sub in hard.SubHardware)
                    {
                        sub.Update();
                    }

                    foreach (var sensor in hard.Sensors)
                    {
                        list.Add(sensor);
                    }
                }
            });
            return list;
        }

        public void Dispose()
        {
            _com.Close();
        }

    }
}
