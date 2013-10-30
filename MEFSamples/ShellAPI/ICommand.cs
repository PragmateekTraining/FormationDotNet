namespace Shell
{
    public interface ICommand
    {
        string Name { get; }

        string Documentation { get; }

        void Execute(IShell shell, string[] parameters);
    }
}
