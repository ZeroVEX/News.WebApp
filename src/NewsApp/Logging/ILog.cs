using System;

namespace Logging
{
    public interface ILog
    {
        void Log(LoggingLevel logLevel, Exception exception, string message, params object[] args);
    }
}