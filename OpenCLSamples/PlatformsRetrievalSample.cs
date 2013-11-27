using Cloo;
using SamplesAPI;
using System;

namespace OpenCLSamples
{
    public class PlatformsRetrievalSample : ISample
    {
        public void Run()
        {
            foreach (ComputePlatform platform in ComputePlatform.Platforms)
            {
                Console.WriteLine("=== {0} ===", platform.Name);

                const string format = "|{0,6}|{1,5}|{2,9}|{3,10}|{4}";

                Console.WriteLine(format, "Device", "Units", "Frequency", "Memory", "Name");

                foreach (ComputeDevice device in platform.Devices)
                {
                    Console.WriteLine(format, device.Type, device.MaxComputeUnits, device.MaxClockFrequency, device.GlobalMemorySize, device.Name);
                }
            }
        }
    }
}
