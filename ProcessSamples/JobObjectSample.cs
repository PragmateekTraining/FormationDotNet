using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SamplesAPI;
using System.Diagnostics;

namespace ProcessSamples
{
    public class JobObjectSample : ISample
    {
        private bool createJobObject = false;

        public JobObjectSample(bool createJobObject)
        {
            this.createJobObject = createJobObject;
        }

        public void Run()
        {
            JobObject jobObject = null;

            try
            {
                if (createJobObject)
                {
                    Console.WriteLine("Creating new job object.");

                    jobObject = new JobObject();
                    jobObject.AddProcess(Process.GetCurrentProcess().Handle);
                }

                Process.Start("cmd");

                Console.WriteLine("Press enter to return...");
                Console.ReadLine();
            }
            finally
            {
                if (jobObject != null)
                {
                    jobObject.Dispose();
                }
            }
        }
    }
}
