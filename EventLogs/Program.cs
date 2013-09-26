namespace EventLogs
{
    class Program
    {
        const string SourceName = "NightlyBatch";

        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                if (args[0] == "--create")
                {
                    new CreationSample(SourceName).Run();
                }
                else if (args[0] == "--delete")
                {
                    new DeletionSample(SourceName).Run();
                }
                else if (args[0] == "--monitor")
                {
                    new MonitoringSample(SourceName).Run();
                }

                return;
            }

            new LoggingSample(SourceName).Run();
        }
    }
}
