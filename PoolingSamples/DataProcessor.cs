using Microsoft.Office.Interop.Excel;
using System;

namespace PoolingSamples
{
    class DataProcessor
    {
        public string File { get; private set; }

        public DataProcessor(string file)
        {
            File = file;
        }

        public void Run()
        {
            using (IManagedApplication managedApplication = ApplicationFactory.Get())
            {
                Application XL = managedApplication.Application;

                Workbook workbook = null;

                try
                {
                    workbook = XL.Workbooks.Open(File);

                    Worksheet sheet = workbook.Worksheets[1];

                    sheet.Cells[1, 1].Value = DateTime.UtcNow.ToString("o");

                    workbook.Close(SaveChanges: true);
                }
                finally
                {
                    if (workbook != null)
                    {
                        workbook.Close(SaveChanges: false);
                    }
                }
            }
        }
    }
}
