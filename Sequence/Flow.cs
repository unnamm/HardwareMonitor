using Common.Config;
using Common.Message;
using Common.Model;
using CommunityToolkit.Mvvm.Messaging;
using Function;
using LibreHardwareMonitor.Hardware;

namespace Sequence
{
    public class Flow : IRecipient<InitCompleteMessage>
    {
        private readonly MonitorHW _hw;
        private readonly int _tick;

        public Flow(MonitorHW hw, DataConfig config)
        {
            WeakReferenceMessenger.Default.RegisterAll(this);

            _hw = hw;
            _tick = config.ReadTick;
        }

        public async void Receive(InitCompleteMessage message)
        {
            await _hw.Open();
            while (true)
            {
                var datas = await _hw.Read();

                List<MonitorData> list = [];
                foreach (var item in datas)
                {
                    list.Add(Convert(item));
                }

                WeakReferenceMessenger.Default.Send(new HardwareInfoMessage(list));
                await Task.Delay(_tick);
            }
        }

        public static MonitorData Convert(ISensor sensor)
        {
            const double FAIL_VALUE = -1d;

            MonitorData item = new();
            item.Name = sensor.Name;
            item.SensorType = sensor.SensorType.ToString();
            item.HardwareType = sensor.Hardware.HardwareType.ToString();
            item.Value = sensor.Value ?? FAIL_VALUE;

            item.Amount = sensor.SensorType switch
            {
                SensorType.Load => "%",
                SensorType.Temperature => "°C",
                SensorType.Clock => "MHz",
                SensorType.Voltage => "V",
                SensorType.Data => sensor.Hardware.HardwareType == HardwareType.Memory ? "GB" : string.Empty,
                _ => string.Empty
            };

            return item;
        }
    }
}
