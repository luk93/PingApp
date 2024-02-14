using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Tools
{
    public static class ExcelManager
    {
        public static ExcelPackage? CreateExcelFile(string path)
        {
            var file = new FileInfo(path);
            if (file.Exists)
            {
                try
                {
                    file.Delete();
                }
                catch
                {
                    return null;
                }
            }
            return new ExcelPackage(file);
        }
        public static async Task SaveExcelFile(ExcelPackage excelPackage)
        {
            await excelPackage.SaveAsync();
            excelPackage.Dispose();
        }
    }
}
