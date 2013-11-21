using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TPLSamples
{
    public class TaskCompletionSourceSample : ISample
    {
        IList<decimal> F = new List<decimal> { 0, 1 };

        Task<decimal> FiboAsync(int n)
        {
            Task<decimal> task = null;

            if (n < F.Count)
            {
                Console.WriteLine("Faking F({0})", n);
                TaskCompletionSource<decimal> tcs = new TaskCompletionSource<decimal>();
                tcs.SetResult(F[n]);
                task = tcs.Task;
            }
            else
            {
                Console.WriteLine("Computing F({0})", n);
                task = Task.Factory.StartNew(() =>
                    {
                        decimal Fi = 0;

                        for (int i = F.Count; i <= n; ++i)
                        {
                            Fi = F[i - 1] + F[i - 2];
                            F.Add(Fi);
                        }

                        return Fi;
                    });
            }

            return task;
        }

        public void Run()
        {
            Console.WriteLine("F(3): {0}", FiboAsync(3).Result);
            Console.WriteLine("F(5): {0}", FiboAsync(5).Result);
            Console.WriteLine("F(10): {0}", FiboAsync(10).Result);
            Console.WriteLine("F(2): {0}", FiboAsync(2).Result);
        }
    }
}
