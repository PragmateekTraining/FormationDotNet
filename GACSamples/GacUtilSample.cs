using SamplesAPI;

namespace GACSamples
{
    public class GacUtilSample : ISample
    {
        public void Run()
        {
            Tools.RunAndWait("build.bat");
            Tools.RunAndWait("roundtrip.bat");
        }
    }
}
