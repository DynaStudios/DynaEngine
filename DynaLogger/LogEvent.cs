using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynaStudios.DynaLogger
{
    public class LogEvent
    {
        public enum LogLevel { Debug, Info, Warn, Error, Fatal };

        public String Message;
        public LogLevel Level;

        public LogEvent(LogLevel level, String message)
        {
            this.Level = level;
            this.Message = message;
        }
    }
}
