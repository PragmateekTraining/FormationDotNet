using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValueTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            new BulkAllocation().MeasureOverhead();
        }
    }
}
