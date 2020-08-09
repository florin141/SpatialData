using System;

namespace Spatial.Data
{
    public class DatabaseLogWriter
    {
        private readonly string _fileName;

        public DatabaseLogWriter()
        {
            _fileName = "DatabaseLog" + "-" + DateTimeOffset.Now.Date.ToString("yyyy-MM-d") + ".log";
        }

        public void Log(string obj)
        {
            //string logLine = DateTime.Now.ToString() + "\t" + level + "\t" + message + Environment.NewLine;
            System.IO.File.AppendAllText(_fileName, obj);
        }
    }
}
