using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynaStudios.DynaLogger.Appender;

namespace DynaStudios.DynaLogger.Appender
{
    public class ConsoleLogger : LoggingAppender
    {

        public void Handle(LogEvent logEvent)
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

        public void Debug(string msg)
        {
            Console.WriteLine("[DEBUG] " + msg);
        }

        public void Info(string msg)
        {
            Console.WriteLine("[INFO] " + msg);
        }

        public void Warn(string msg)
        {
            Console.WriteLine("[WARN] " + msg);
        }

        public void Error(string msg)
        {
            Console.WriteLine("[ERROR] " + msg);
        }

        public void Fatal(string msg)
        {
            Console.WriteLine("[FATAL] " + msg);
        }
    }
}
