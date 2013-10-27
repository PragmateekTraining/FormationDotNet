using Logging;
using SamplesAPI;

namespace DependencyWalkerSamples
{
    public class DependenciesTrackingSample : ISample
    {
        string message;

        public DependenciesTrackingSample(string message)
        {
            this.message = message;
        }

        public void Run()
        {
            Logger.Log(message);
        }
    }
}
