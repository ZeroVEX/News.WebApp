using System;

namespace Logging
{
    public static class LoggerExtensions
    {
        public static void Log(this ILog logger, LoggingLevel logLevel, string message, params object[] args)
        {
            logger.Log(logLevel, null, message, args);
        }

        public static void LogTrace(this ILog logger, Exception exception, string message, params object[] args)
        {
            logger.Log(LoggingLevel.Trace, exception, message, args);
        }

        public static void LogTrace(this ILog logger, string message, params object[] args)
        {
            logger.Log(LoggingLevel.Trace, null, message, args);
        }

        public static void LogDebug(this ILog logger, Exception exception, string message, params object[] args)
        {
            logger.Log(LoggingLevel.Debug, exception, message, args);
        }

        public static void LogDebug(this ILog logger, string message, params object[] args)
        {
            logger.Log(LoggingLevel.Debug, null, message, args);
        }

        public static void LogInformation(this ILog logger, Exception exception, string message, params object[] args)
        {
            logger.Log(LoggingLevel.Information, exception, message, args);
        }

        public static void LogInformation(this ILog logger, string message, params object[] args)
        {
            logger.Log(LoggingLevel.Information, null, message, args);
        }

        public static void LogWarning(this ILog logger, Exception exception, string message, params object[] args)
        {
            logger.Log(LoggingLevel.Warning, exception, message, args);
        }

        public static void LogWarning(this ILog logger, string message, params object[] args)
        {
            logger.Log(LoggingLevel.Warning, null, message, args);
        }

        public static void LogError(this ILog logger, Exception exception, string message, params object[] args)
        {
            logger.Log(LoggingLevel.Error, exception, message, args);
        }

        public static void LogError(this ILog logger, string message, params object[] args)
        {
            logger.Log(LoggingLevel.Error, null, message, args);
        }

        public static void LogCritical(this ILog logger, Exception exception, string message, params object[] args)
        {
            logger.Log(LoggingLevel.Critical, exception, message, args);
        }

        public static void LogCritical(this ILog logger, string message, params object[] args)
        {
            logger.Log(LoggingLevel.Critical, null, message, args);
        }
    }
}