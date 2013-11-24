namespace RegexSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new AnchorsSample().Run();
            // new BacktrackingSample().Run();
            // new CompiledRegexSample().Run();
            new DisabledBacktrackingSample(args.Length == 1 && args[0] == "check").Run();
            // new ExponentialBacktrackingSample().Run();            
        }
    }
}
