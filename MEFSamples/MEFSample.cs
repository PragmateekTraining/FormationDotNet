using SamplesAPI;

namespace MEFSamples
{
    public class MEFSample : ISample
    {
        public void Run()
        {
            new ExtensibleShell().Run();
        }
    }
}
