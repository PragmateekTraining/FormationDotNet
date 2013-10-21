using SamplesAPI;

namespace CERSamples
{
    public class CERWithHostingSample : ISample
    {
        private readonly string sampleName;

        public CERWithHostingSample(string sampleName)
        {
            this.sampleName = sampleName;
        }

        public void Run()
        {
            Tools.RunAndWait("CLRHost.exe", sampleName);
        }
    }
}
