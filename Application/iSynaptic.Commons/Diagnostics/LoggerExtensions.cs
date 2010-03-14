using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Diagnostics
{
    public static class LoggerExtensions
    {
        private static void SaveLog<T>(this ILogger logger, LogLevel level, object message, T context)
        {
            if (logger != null)
                logger.Log<T>(level, message, context);
        }

        public static void Debug(this ILogger logger, object message)
        {
            logger.Debug<object>(message, null);
        }

        public static void Debug<T>(this ILogger logger, object message, T context)
        {
            logger.SaveLog(LogLevel.Debug, message, context);
        }

        public static void Info(this ILogger logger, object message)
        {
            logger.Info<object>(message, null);
        }

        public static void Info<T>(this ILogger logger, object message, T context)
        {
            logger.SaveLog(LogLevel.Info, message, context);
        }

        public static void Warn(this ILogger logger, object message)
        {
            logger.Warn<object>(message, null);
        }

        public static void Warn<T>(this ILogger logger, object message, T context)
        {
            logger.SaveLog(LogLevel.Warn, message, context);
        }

        public static void Error(this ILogger logger, object message)
        {
            logger.Error<object>(message, null);
        }

        public static void Error<T>(this ILogger logger, object message, T context)
        {
            logger.SaveLog(LogLevel.Error, message, context);
        }

        public static void Fatal(this ILogger logger, object message)
        {
            logger.Fatal<object>(message, null);
        }

        public static void Fatal<T>(this ILogger logger, object message, T context)
        {
            logger.SaveLog(LogLevel.Fatal, message, context);
        }
    }
}
