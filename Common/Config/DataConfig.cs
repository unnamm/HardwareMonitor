
namespace Common.Config
{
    public class DataConfig : ConfigBase
    {
        public int OpenTimeout; //hardware monitor open delay time
        public int ReadTick; //read time interval, millisceond

        public DataConfig()
        {
            base.Get(ref OpenTimeout, "data");
            base.Get(ref ReadTick, "data");
        }
    }
}
