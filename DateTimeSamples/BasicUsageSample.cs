using SamplesAPI;
using System;

namespace DateTimeSamples
{
    public class BasicUsageSample : ISample
    {
        public void Run()
        {
            DateTime localNow = DateTime.Now;
            string localNowString = localNow.ToString("o");

            DateTime UtcNow = DateTime.UtcNow;
            string UtcNowString = UtcNow.ToString("o");

            DateTime unspecifiedNow = new DateTime(localNow.Ticks, DateTimeKind.Unspecified);
            string unspecifiedNowString = unspecifiedNow.ToString("o");

            DateTimeOffset nowWithOffset = DateTimeOffset.Now;
            string nowWithOffsetString = nowWithOffset.ToString("o");

            Console.WriteLine("Local:       " + localNowString);
            Console.WriteLine("Utc:         " + UtcNowString);
            Console.WriteLine("Unspecified: " + unspecifiedNowString);
            Console.WriteLine("With offset: " + nowWithOffsetString);
        }
    }
}
