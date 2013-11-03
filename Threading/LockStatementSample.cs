using SamplesAPI;

namespace ThreadingSamples
{
    public class LockStatementSample : ISample
    {
        public void Run()
        {
            object state = new object();

            lock (state)
            {
            }
        }
    }
}
