using System;
using System.IO;

namespace DynaStudios.DynaLogger.Appender
{
    public class FileSystemLogger : LoggingAppender
    {

        private StreamWriter _writer;

        public FileSystemLogger()
        {

            String folderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if (!Directory.Exists(folderPath + @"\Logs"))
            {
                System.IO.Directory.CreateDirectory(folderPath + @"\Logs");
            }

            _writer = new StreamWriter(folderPath + @"\Logs\log.txt");
            _writer.AutoFlush = true;
        }

        public void Handle(LogEvent logEvent)
        {
            if (_writer != null)
            {
                switch (logEvent.Level)
                {
                    case LogEvent.LogLevel.Debug:
                        Debug(logEvent.Message);
                        break;
                    case LogEvent.LogLevel.Info:
                        Info(logEvent.Message);
                        break;
                    case LogEvent.LogLevel.Warn:
                        Warn(logEvent.Message);
                        break;
                    case LogEvent.LogLevel.Error:
                        Error(logEvent.Message);
                        break;
                    case LogEvent.LogLevel.Fatal:
                        Fatal(logEvent.Message);
                        break;
                }
            }
        }

        public void Debug(string msg)
        {
            _writer.WriteLine(Convert.ToString(System.DateTime.Now) + "[DEBUG] " + msg);
        }

        public void Info(string msg)
        {
            _writer.WriteLine(Convert.ToString(System.DateTime.Now) + "[INFO] " + msg);
        }

        public void Warn(string msg)
        {
            _writer.WriteLine(Convert.ToString(System.DateTime.Now) + "[WARN] " + msg);
        }

        public void Error(string msg)
        {
            _writer.WriteLine(Convert.ToString(System.DateTime.Now) + "[ERROR] " + msg);
        }

        public void Fatal(string msg)
        {
            _writer.WriteLine(Convert.ToString(System.DateTime.Now) + "[FATAL] " + msg);
        }
    }
}
