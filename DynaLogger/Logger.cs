using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynaStudios.DynaLogger.Appender;

namespace DynaStudios.DynaLogger
{

    public delegate void NewLogReceived(LogEvent logEvent);

    public class Logger
    {

        public NewLogReceived LogReceived;
        
        private List<LoggingAppender> _appenders;

        public Logger()
        {
            _appenders = new List<LoggingAppender>();
            LogReceived = new NewLogReceived(LogFire);
        }

        public void Debug(String message)
        {
            LogEvent log = new LogEvent(LogEvent.LogLevel.Debug, message);
            LogReceived(log);
        }

        public void Info(String message)
        {
            LogEvent log = new LogEvent(LogEvent.LogLevel.Info, message);
            LogReceived(log);
        }

        public void Warn(String message)
        {
            LogEvent log = new LogEvent(LogEvent.LogLevel.Warn, message);
            LogReceived(log);
        }

        public void Error(String message)
        {
            LogEvent log = new LogEvent(LogEvent.LogLevel.Error, message);
            LogReceived(log);
        }

        public void Fatal(String message)
        {
            LogEvent log = new LogEvent(LogEvent.LogLevel.Fatal, message);
            LogReceived(log);
        }

        public void Register(LoggingAppender appender)
        {
            _appenders.Add(appender);
            LogReceived += new NewLogReceived(appender.Handle);
        }

        private void LogFire(LogEvent logEvent) { }

    }
}
