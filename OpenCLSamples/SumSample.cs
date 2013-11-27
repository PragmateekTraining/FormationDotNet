using Cloo;
using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCLSamples
{
    public class SumSample : ISample
    {
        public void Run()
        {
            const long n = 1 << 24;

            float[] a = new float[n];
            float[] b = new float[n];
            float[] sum = new float[n];
            float[] sumWithLocal = new float[n];
            float[] thSum = new float[n];

            Random rand = new Random();
            for (int i = 0; i < n; ++i)
            {
                a[i] = rand.Next(-10, +11);
                b[i] = rand.Next(-10, +11);
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < n; ++i)
            {
                thSum[i] = (float)(Math.Log(Math.Sqrt(Math.Exp(Math.Cos(Math.Sin(a[i]))))) + Math.Log(Math.Sqrt(Math.Exp(Math.Cos(Math.Sin(b[i]))))));
            }
            stopwatch.Stop();

            TimeSpan time = stopwatch.Elapsed;

            ComputeDevice GPU = ComputePlatform.Platforms
                                               .Cast<ComputePlatform>()
                                               .SelectMany(p => p.Devices)
                                               .Single(d => d.Type == ComputeDeviceTypes.Gpu);

            ComputeContext context = new ComputeContext(new[] { GPU }, new ComputeContextPropertyList(ComputePlatform.Platforms.Single()), null, IntPtr.Zero);

            string programCode =
@"__kernel void sum(__global float* a, __global float* b, __global float* c)
{
    __private size_t gid = get_global_id(0);

    c[gid] = log(sqrt(exp(cos(sin(a[gid]))))) + log(sqrt(exp(cos(sin(b[gid])))));
}

__kernel void sum_with_local_copy(__global float* a, __global float* b, __global float* c, __local float* tmpa, __local float* tmpb, __local float* tmpc)
{
    __private size_t gid = get_global_id(0);
    __private size_t lid = get_local_id(0);
    __private size_t grid = get_group_id(0);
    __private size_t lsz = get_local_size(0);

    event_t evta = async_work_group_copy(tmpa, a + grid * lsz, lsz, 0);
    wait_group_events(1, &evta);

    event_t evtb = async_work_group_copy(tmpb, b + grid * lsz, lsz, 0);
    wait_group_events(1, &evtb);

    tmpc[lid] = log(sqrt(exp(cos(sin(tmpa[lid]))))) + log(sqrt(exp(cos(sin(tmpb[lid])))));
    
    event_t evt = async_work_group_copy(c + grid * lsz, tmpc, lsz, 0);
    wait_group_events(1, &evt);
}";

            ComputeProgram program = new ComputeProgram(context, programCode);
            program.Build(new[] { GPU }, null, null, IntPtr.Zero);

            ComputeKernel sumKernel = program.CreateKernel("sum");
            ComputeKernel sumWithLocalCopyKernel = program.CreateKernel("sum_with_local_copy");

            ComputeCommandQueue queue = new ComputeCommandQueue(context, GPU, ComputeCommandQueueFlags.None);

            ComputeBuffer<float> aBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.UseHostPointer | ComputeMemoryFlags.ReadOnly, a);
            ComputeBuffer<float> bBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.UseHostPointer | ComputeMemoryFlags.ReadOnly, b);
            ComputeBuffer<float> sumBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.UseHostPointer | ComputeMemoryFlags.WriteOnly, sum);
            ComputeBuffer<float> sumWithLocalCopyBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.UseHostPointer | ComputeMemoryFlags.WriteOnly, sumWithLocal);

            long localSize = GPU.MaxWorkGroupSize;

            sumKernel.SetMemoryArgument(0, aBuffer);
            sumKernel.SetMemoryArgument(1, bBuffer);
            sumKernel.SetMemoryArgument(2, sumBuffer);

            sumWithLocalCopyKernel.SetMemoryArgument(0, aBuffer);
            sumWithLocalCopyKernel.SetMemoryArgument(1, bBuffer);
            sumWithLocalCopyKernel.SetMemoryArgument(2, sumWithLocalCopyBuffer);
            sumWithLocalCopyKernel.SetLocalArgument(3, localSize);
            sumWithLocalCopyKernel.SetLocalArgument(4, localSize);
            sumWithLocalCopyKernel.SetLocalArgument(5, localSize);

            stopwatch.Restart();
            queue.Execute(sumWithLocalCopyKernel, null, new[] { n }, new[] { localSize }, null);

            queue.ReadFromBuffer<float>(sumWithLocalCopyBuffer, ref sumWithLocal, true, null);
            stopwatch.Stop();

            TimeSpan GPUWithLocalCopyTime = stopwatch.Elapsed;

            stopwatch.Restart();
            queue.Execute(sumKernel, null, new[] { n }, new[] { localSize }, null);

            queue.ReadFromBuffer<float>(sumBuffer, ref sum, true, null);
            stopwatch.Stop();

            TimeSpan GPUTime = stopwatch.Elapsed;

            Console.WriteLine("CPU: {0}", time);
            Console.WriteLine("GPU: {0}", GPUTime);
            Console.WriteLine("GPU with local copy: {0}", GPUWithLocalCopyTime);

            for (int i = 0; i < n; ++i)
            {
                if (Math.Abs(sum[i] - thSum[i]) > 1e-6)
                {
                    Console.Error.WriteLine("GPU1: Values differ at {0}: {1} vs {2}!", i, sum[i], thSum[i]);
                    break;
                }

                if (Math.Abs(sumWithLocal[i] - thSum[i]) > 1e-6)
                {
                    Console.Error.WriteLine("GPU2: Values differ at {0}: {1} vs {2}!", i, sumWithLocal[i], thSum[i]);
                    double d = thSum.FirstOrDefault(f => sumWithLocal[i] - f < 1e-5);
                    int match = Array.IndexOf(thSum, d);
                    break;
                }
            }
        }
    }
}
