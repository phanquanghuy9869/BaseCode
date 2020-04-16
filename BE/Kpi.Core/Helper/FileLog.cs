using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kpi.Core.Helper
{
    public static class FileLog
    {
        public static void WriteLog(string message)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "\\logs\\" + DateTime.Today.ToString("dd-MM-yyyy") + ".txt";
            var str = DateTime.Now.ToString("HH:mm:ss") + ": " + message + Environment.NewLine;
            File.AppendAllText(path, str);
        }
    }
}
