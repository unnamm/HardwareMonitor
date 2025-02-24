using Common.Config;
using LibreHardwareMonitor.Hardware;
using System.Text;
using System.Threading;

namespace Function
{
    public class MonitorHW
    {
        private readonly Computer _com;
        private readonly int _timeout;

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

        public async Task<string> Read()
        {
            const string FAIL_VALUE = "-1";

            var sb = new StringBuilder();

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
                        string value = sensor.Value.HasValue ? sensor.Value.Value.ToString() : FAIL_VALUE;
                        string text = $"{sensor.Name} {sensor.SensorType} = {value}";
                        sb.AppendLine(text);
                    }
                }
            });
            return sb.ToString();
        }
    }
}
