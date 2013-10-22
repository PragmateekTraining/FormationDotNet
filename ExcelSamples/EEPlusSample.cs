using Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using SamplesAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelSamples
{
    public static class SheetExtensions
    {
        public static ICell GetCell(this ISheet sheet, int rowIndex, int columnIndex)
        {
            ICell cell = null;

            IRow row = sheet.GetRow(rowIndex);

            if (row != null)
            {
                cell = row.GetCell(columnIndex);
            }

            return cell;
        }
    }

    public class EEPlusSample : ISample
    {
        void WriteWithNPOI(int[,] data, out TimeSpan time)
        {
            const string NPOIFileName = "NPOI.xlsx";

            if (File.Exists(NPOIFileName)) File.Delete(NPOIFileName);

            Stopwatch stopwatch = Stopwatch.StartNew();

            IWorkbook workbook = new HSSFWorkbook();

            ISheet sheet = workbook.CreateSheet("data");

            for (int iRow = 1; iRow <= data.GetLength(0); ++iRow)
            {
                IRow row = sheet.CreateRow(iRow);

                for (int iCol = 1; iCol <= data.GetLength(1); ++iCol)
                {
                    row.CreateCell(iCol).SetCellValue(data[iRow - 1, iCol - 1]);
                }
            }

            using (FileStream file = File.Create(NPOIFileName))
            {
                workbook.Write(file);
            }

            stopwatch.Stop();

            time = stopwatch.Elapsed;
        }

        void WriteWithEEPlus(int[,] data, out TimeSpan time)
        {
            const string EEPlusFileName = "EEPlus.xlsx";

            if (File.Exists(EEPlusFileName)) File.Delete(EEPlusFileName);

            Stopwatch stopwatch = Stopwatch.StartNew();
            using (ExcelPackage package = new ExcelPackage(new FileInfo(EEPlusFileName)))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("data");

                /* sheet.Cells[data.GetLength(0), data.GetLength(1)].Value = 0;

                for (int row = 1; row <= data.GetLength(0); ++row)
                {
                    for (int col = 1; col <= data.GetLength(1); ++col)
                    {
                        sheet.Cells[row, col].Value = data[row - 1, col - 1];
                    }
                }*/

                sheet.Cells[1, 1, data.GetLength(0), data.GetLength(1)].Value = data;

                for (int col = 1; col <= data.GetLength(1); ++col)
                {
                    sheet.Column(col).AutoFit();
                }

                package.Save();
            }
            stopwatch.Stop();

            time = stopwatch.Elapsed;
        }

        void WriteWithExcel(int[,] data, out TimeSpan time)
        {
            const string ExcelFileName = "Excel.xlsx";

            if (File.Exists(ExcelFileName)) File.Delete(ExcelFileName);

            Stopwatch stopwatch = Stopwatch.StartNew();

            Application Excel = null;
            Workbook workbook = null;
            try
            {
                Excel = new Application();

                workbook = Excel.Workbooks.Add();

                Worksheet sheet = workbook.Sheets.Add();
                sheet.Name = "data";
                sheet.Range[sheet.Cells[1, 1], sheet.Cells[data.GetLength(0), data.GetLength(1)]].Value = data;
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.SaveAs(Path.Combine(Directory.GetCurrentDirectory(), ExcelFileName));
                    workbook.Close(false);
                }

                if (Excel != null)
                {
                    Excel.Quit();
                }
            }

            stopwatch.Stop();

            time = stopwatch.Elapsed;
        }

        public void Run()
        {
            Console.Write("m: ");
            int m = Convert.ToInt32(Console.ReadLine());
            Console.Write("n: ");
            int n = Convert.ToInt32(Console.ReadLine());

            int[,] data = new int[m, n];

            Random rand = new Random();
            for (int row = 0; row < m; ++row)
            {
                for (int col = 0; col < n; ++col)
                {
                    data[row, col] = rand.Next();
                }
            }

            TimeSpan withNPOITime, withEEPlusTime, withExcelTime;

            WriteWithNPOI(data, out withNPOITime);
            WriteWithEEPlus(data, out withEEPlusTime);
            WriteWithExcel(data, out withExcelTime);

            Console.WriteLine("With NPOI: {0}", withNPOITime);
            Console.WriteLine("With EEPlus: {0}", withEEPlusTime);
            Console.WriteLine("With Excel: {0}", withExcelTime);
        }
    }
}
