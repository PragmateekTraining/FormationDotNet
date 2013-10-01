using SamplesAPI;

namespace CILSamples
{
    public class HelloWorldSample : ISample
    {
        public void Run()
        {
            Tools.RunAndWait("HelloWorld.exe");
        }
    }
}
