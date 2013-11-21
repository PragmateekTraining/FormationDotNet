using SamplesAPI;
using System;
using System.Threading.Tasks;

namespace AsyncSamples
{
    public class TaskRunUnwrapSample : ISample
    {
        public void Run()
        {
            int a = 1;
            int b = 2;

            Func<Task<int>> add = async () =>
                {
                    await Task.Delay(0);

                    return a + b;
                };

            Task<Task<int>> startNewTask = Task.Factory.StartNew(add);
            Task<int> unwrappedStartNewTask = Task.Factory.StartNew(add).Unwrap();
            Task<int> taskRunTask = Task.Run(add);

            Console.WriteLine(startNewTask.Result.Result);
            Console.WriteLine(unwrappedStartNewTask.Result);
            Console.WriteLine(taskRunTask.Result);
        }
    }
}
