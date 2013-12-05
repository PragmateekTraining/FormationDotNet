using SamplesAPI;

namespace GenericsSamples
{
    /// <summary>
    /// Disassemble the current program to show how foreach is implemented: using "castclass" instructions when using a non type-safe collection.
    /// </summary>
    public class IldasmSample : ISample
    {
        public void Run()
        {
            Tools.RunAndWait(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\ildasm.exe", "GenericsSamples.exe");
        }
    }
}
