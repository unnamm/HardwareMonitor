
namespace Common.Config
{
    public class DataConfig : ConfigBase
    {
        public int OpenTimeout;

        public DataConfig()
        {
            base.Get(ref OpenTimeout, "data");
        }
    }
}
