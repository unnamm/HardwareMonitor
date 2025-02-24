using Common.Message;
using CommunityToolkit.Mvvm.Messaging;
using Function;

namespace Sequence
{
    public class Flow : IRecipient<InitCompleteMessage>
    {
        private readonly MonitorHW _hw;

        public Flow(MonitorHW hw)
        {
            _hw = hw;
            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        public async void Receive(InitCompleteMessage message)
        {
            await _hw.Open();
            while (true)
            {
                var result = await _hw.Read();
                WeakReferenceMessenger.Default.Send(new HardwareInfoMessage(result));
                await Task.Delay(1000);
            }
        }
    }
}
