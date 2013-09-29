using SamplesAPI;

namespace ClrSamples
{
    public class UninitializedMemorySample : ISample
    {
        public void Run()
        {
            Tools.RunAndWait("uninitialized.exe");
        }
    }
}
