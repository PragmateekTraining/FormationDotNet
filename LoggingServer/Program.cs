namespace Logging
{
    class Program
    {
        static void Main(string[] args)
        {
            new Server().Start(int.Parse(args[0]));
        }
    }
}
