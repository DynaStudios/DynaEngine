using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynaStudios.DynaLogger.Appender
{
    public interface LoggingAppender
    {

        void Handle(LogEvent logEvent);

        void Debug(String msg);

        void Info(String msg);

        void Warn(String msg);

        void Error(String msg);

        void Fatal(String msg);

    }
}
