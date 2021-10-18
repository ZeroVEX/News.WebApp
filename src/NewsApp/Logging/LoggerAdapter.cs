using System;
using Microsoft.Extensions.Logging;

namespace Logging
{
    public class LoggerAdapter : ILog
    {
        private readonly ILogger _logger;


        public LoggerAdapter(ILogger logger)
        {
            _logger = logger;
        }


        public void Log(LoggingLevel logLevel, Exception exception, string message, params object[] args)
        {
            _logger.Log((LogLevel)logLevel, exception, message, args);
        }
    }
}