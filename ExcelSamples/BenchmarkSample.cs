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
    /*public static class SheetExtensions
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
    }*/

    public class BenchmarkSample : ISample
    {
        const string NPOIFileName = "NPOI.xlsx";
        const string EPPlusFileName = "EPPlus.xlsx";
        const string ExcelFileName = "Excel.xlsx";

        void WriteWithNPOI(int[,] data, out TimeSpan time)
        {
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

        int[,] ReadWithNPOI(out TimeSpan time)
        {
            int[,] data = null;

            Stopwatch stopwatch = Stopwatch.StartNew();

            using (FileStream file = File.Open(NPOIFileName, FileMode.Open))
            {
                IWorkbook workbook = new HSSFWorkbook(file);

                ISheet sheet = workbook.GetSheet("data");

                int m = 0;
                int n = 0;

                while (true)
                {
                    IRow row = sheet.GetRow(m + 1);

                    if (row == null) break;

                    ++m;
                }

                IRow firstRow = sheet.GetRow(1);
                while (true)
                {
                    ICell cell = firstRow.GetCell(n + 1);

                    if (cell == null || cell.CellType != CellType.NUMERIC) break;

                    ++n;
                }

                data = new int[m, n];

                for (int iRow = 1; iRow <= m; ++iRow)
                {
                    IRow row = sheet.GetRow(iRow);

                    for (int iCol = 1; iCol <= n; ++iCol)
                    {
                        data[iRow - 1, iCol - 1] = (int)row.GetCell(iCol).NumericCellValue;
                    }
                }
            }

            stopwatch.Stop();

            time = stopwatch.Elapsed;

            return data;
        }

        void WriteWithEPPlus(int[,] data, out TimeSpan time)
        {
            if (File.Exists(EPPlusFileName)) File.Delete(EPPlusFileName);

            Stopwatch stopwatch = Stopwatch.StartNew();
            using (ExcelPackage package = new ExcelPackage(new FileInfo(EPPlusFileName)))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("data");

                for (int row = 1; row <= data.GetLength(0); ++row)
                {
                    for (int col = 1; col <= data.GetLength(1); ++col)
                    {
                        sheet.Cells[row, col].Value = data[row - 1, col - 1];
                    }
                }

                // sheet.Cells[1, 1, data.GetLength(0), data.GetLength(1)].Value = data;

                for (int col = 1; col <= data.GetLength(1); ++col)
                {
                    sheet.Column(col).AutoFit();
                }

                package.Save();
            }
            stopwatch.Stop();

            time = stopwatch.Elapsed;
        }

        int[,] ReadWithEPPlus(out TimeSpan time)
        {
            int[,] data;

            Stopwatch stopwatch = Stopwatch.StartNew();
            using (ExcelPackage package = new ExcelPackage(new FileInfo(EPPlusFileName)))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets["data"];

                int m = sheet.Dimension.End.Row;
                int n = sheet.Dimension.End.Column;

                data = new int[m, n];

                /* sheet.Cells[data.GetLength(0), data.GetLength(1)].Value = 0;

                for (int row = 1; row <= data.GetLength(0); ++row)
                {
                    for (int col = 1; col <= data.GetLength(1); ++col)
                    {
                        sheet.Cells[row, col].Value = data[row - 1, col - 1];
                    }
                }*/

                object[,] values = sheet.Cells[1, 1, m, n].Value as object[,];

                for (int iRow = 0; iRow < m; ++iRow)
                {
                    for (int iCol = 0; iCol < n; ++iCol)
                    {
                        data[iRow, iCol] = (int)(double)values[iRow, iCol];
                    }
                }
            }
            stopwatch.Stop();

            time = stopwatch.Elapsed;

            return data;
        }

        void WriteWithExcel(int[,] data, out TimeSpan time)
        {
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

        int[,] ReadWithExcel(out TimeSpan time)
        {
            int[,] data = null;

            Stopwatch stopwatch = Stopwatch.StartNew();

            Application Excel = null;
            Workbook workbook = null;
            try
            {
                Excel = new Application();

                workbook = Excel.Workbooks.Open(Path.Combine(Directory.GetCurrentDirectory(), ExcelFileName));

                Worksheet sheet = workbook.Sheets["data"];

                int m = sheet.UsedRange.Rows.Count;
                int n = sheet.UsedRange.Columns.Count;

                data = new int[m,n];

                object[,] values = sheet.UsedRange.Value;

                for (int iRow = 0; iRow < m; ++iRow)
                {
                    for (int iCol = 0; iCol < n; ++iCol)
                    {
                        data[iRow, iCol] = (int)(double)values[iRow + 1, iCol + 1];
                    }
                }
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close(false);
                }

                if (Excel != null)
                {
                    Excel.Quit();
                }
            }

            stopwatch.Stop();

            time = stopwatch.Elapsed;

            return data;
        }

        public static bool AreEquals(int[,] matrix1, int[,] matrix2)
        {
            if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1)) return false;

            for (int iRow = 0; iRow < matrix1.GetLength(0); ++iRow)
            {
                for (int iCol = 0; iCol < matrix1.GetLength(1); ++iCol)
                {
                    if (matrix1[iRow, iCol] != matrix2[iRow, iCol]) return false;
                }
            }

            return true;
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

            TimeSpan withNPOIWriteTime, withEPPlusWriteTime, withExcelWriteTime;

            WriteWithNPOI(data, out withNPOIWriteTime);
            WriteWithEPPlus(data, out withEPPlusWriteTime);
            WriteWithExcel(data, out withExcelWriteTime);

            Console.WriteLine("Write with NPOI: {0}", withNPOIWriteTime);
            Console.WriteLine("Write with EPPlus: {0}", withEPPlusWriteTime);
            Console.WriteLine("Write with Excel: {0}", withExcelWriteTime);

            TimeSpan withNPOIReadTime, withEPPlusReadTime, withExcelReadTime;

            int[,] outNPOIData = ReadWithNPOI(out withNPOIReadTime);
            int[,] outEPPlusData = ReadWithEPPlus(out withEPPlusReadTime);
            int[,] outExcelData = ReadWithExcel(out withExcelReadTime);

            Console.WriteLine("Check NPOI: {0}", AreEquals(data, outNPOIData) ? "OK" : "X");
            Console.WriteLine("Check EPPlus: {0}", AreEquals(data, outEPPlusData) ? "OK" : "X");
            Console.WriteLine("Check Excel: {0}", AreEquals(data, outExcelData) ? "OK" : "X");

            Console.WriteLine("Read with NPOI: {0}", withNPOIReadTime);
            Console.WriteLine("Read with EPPlus: {0}", withEPPlusReadTime);
            Console.WriteLine("Read with Excel: {0}", withExcelReadTime);
        }
    }
}
