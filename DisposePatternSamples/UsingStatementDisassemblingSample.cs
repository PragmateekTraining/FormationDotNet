using SamplesAPI;

namespace DisposePatternSamples
{
    public class UsingStatementDisassemblingSample : ISample
    {
        public void Run()
        {
            Tools.RunAndWait(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\ildasm.exe", "DisposePatternSamples.exe");
        }
    }
}
