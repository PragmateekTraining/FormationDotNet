using Cloo;
using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCLSamples
{
    public class AtomicOperationsSample : ISample
    {
        public void Run()
        {
            const long workitemsCount = 123456789;

            ComputeDevice GPU = ComputePlatform.Platforms
                                               .Cast<ComputePlatform>()
                                               .SelectMany(p => p.Devices)
                                               .Single(d => d.Type == ComputeDeviceTypes.Gpu);

            ComputeContext context = new ComputeContext(new[] { GPU }, new ComputeContextPropertyList(ComputePlatform.Platforms.Single()), null, IntPtr.Zero);

            string programCode =
@"__kernel void count(__global uint* result)
{
    *result += 1;
}

__kernel void atomic_count(__global uint* result)
{
    atomic_inc(result);
}";

            ComputeProgram program = new ComputeProgram(context, programCode);
            program.Build(new[] { GPU }, null, null, IntPtr.Zero);

            ComputeKernel countKernel = program.CreateKernel("count");
            ComputeKernel atomicCountKernel = program.CreateKernel("atomic_count");

            ComputeCommandQueue queue = new ComputeCommandQueue(context, GPU, ComputeCommandQueueFlags.None);

            ComputeBuffer<int> countBuffer = new ComputeBuffer<int>(context, ComputeMemoryFlags.AllocateHostPointer, 1);
            countKernel.SetMemoryArgument(0, countBuffer);
            queue.Execute(countKernel, null, new[] { workitemsCount }, null, null);

            int[] result = new int[1];
            queue.ReadFromBuffer<int>(countBuffer, ref result, true, null);

            Console.WriteLine("Result non-atomic : {0}\n", result[0]);

            ComputeBuffer<int> atomicCountBuffer = new ComputeBuffer<int>(context, ComputeMemoryFlags.AllocateHostPointer, 1);
            atomicCountKernel.SetMemoryArgument(0, atomicCountBuffer);
            queue.Execute(atomicCountKernel, null, new[] { workitemsCount }, null, null);

            queue.ReadFromBuffer<int>(atomicCountBuffer, ref result, true, null);

            Console.WriteLine("Result atomic : {0}\n", result[0]);
        }
    }
}
