﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // new JobObjectSample(args.Length == 1 && args[0] == "create-job-object").Run();
            new FailFastSample(args.Length == 1 && args[0] == "failFast").Run();
        }
    }
}
