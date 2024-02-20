using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;

namespace PingApp.Tools
{
    public static class FileTools
    {
        public static bool IsFileLocked(string filePath)
        {
            try
            {
                var stream = File.OpenRead(filePath);
                return false;
            }
            catch (IOException)
            {
                return true;
            }
        }
        public static FileInfo? SelectXlsxFileAndTryToUse(string title)
        {
            OpenFileDialog openFileDialog1 = new()
            {
                InitialDirectory = @"c:\Users\localadm\Desktop",
                Title = title,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "xlsx",
                Filter = "Excel file (*.xlsx)|*.xlsx",
                RestoreDirectory = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true,
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo xlsx = new(openFileDialog1.FileName);
                if (xlsx.Exists && !IsFileLocked(xlsx.FullName))
                {
                    return xlsx;
                }
                Log.Warning($"File '{xlsx.FullName}' not exist or in use!");
                return null;
            }
            Log.Warning($"File not selected!");
            return null;
        }
        public static string GetDateTimeString()
        {
            string dateTime = DateTime.Now.Year.ToString();
            if (DateTime.Now.Month.ToString().Length == 1)
            {
                dateTime += "0" + DateTime.Now.Month.ToString();
            }
            else dateTime += DateTime.Now.Month.ToString();
            if (DateTime.Now.Day.ToString().Length == 1)
            {
                dateTime += "0" + DateTime.Now.Day.ToString();
            }
            else dateTime += DateTime.Now.Day.ToString();
            dateTime += "_";
            if (DateTime.Now.TimeOfDay.Hours.ToString().Length == 1)
            {
                dateTime += "0" + DateTime.Now.TimeOfDay.Hours.ToString();
            }
            else dateTime += DateTime.Now.TimeOfDay.Hours;
            if (DateTime.Now.TimeOfDay.Minutes.ToString().Length == 1)
            {
                dateTime += "0" + DateTime.Now.TimeOfDay.Minutes.ToString();
            }
            else dateTime += DateTime.Now.TimeOfDay.Minutes;
            return dateTime;
        }
        public static string OverridePathWithDateTimeSubfolder(string expFolderPath)
        {
            string dateTime = GetDateTimeString();
            DirectoryInfo directory2 = new(expFolderPath);
            directory2.CreateSubdirectory(dateTime);
            return expFolderPath + @"\" + dateTime;
        }
    }
}
