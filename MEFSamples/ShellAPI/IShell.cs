using System.Collections.Generic;

namespace Shell
{
    public interface IShell
    {
        IEnumerable<ICommand> Commands { get; }

        bool IsRunning { get; }

        void Run();
        void Stop();
    }
}
