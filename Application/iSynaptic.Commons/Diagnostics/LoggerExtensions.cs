﻿namespace iSynaptic.Commons.Diagnostics
{
    public static class LoggerExtensions
    {
        private static void SafeLog(this ILogger logger, LogLevel level, object message, object context)
        {
            if (logger != null)
                logger.Log(level, message, context);
        }

        public static void Debug(this ILogger logger, object message)
        {
            logger.Debug(message, null);
        }

        public static void Debug(this ILogger logger, object message, object context)
        {
            logger.SafeLog(LogLevel.Debug, message, context);
        }

        public static void Info(this ILogger logger, object message)
        {
            logger.Info(message, null);
        }

        public static void Info(this ILogger logger, object message, object context)
        {
            logger.SafeLog(LogLevel.Info, message, context);
        }

        public static void Warn(this ILogger logger, object message)
        {
            logger.Warn(message, null);
        }

        public static void Warn(this ILogger logger, object message, object context)
        {
            logger.SafeLog(LogLevel.Warn, message, context);
        }

        public static void Error(this ILogger logger, object message)
        {
            logger.Error(message, null);
        }

        public static void Error(this ILogger logger, object message, object context)
        {
            logger.SafeLog(LogLevel.Error, message, context);
        }

        public static void Fatal(this ILogger logger, object message)
        {
            logger.Fatal(message, null);
        }

        public static void Fatal(this ILogger logger, object message, object context)
        {
            logger.SafeLog(LogLevel.Fatal, message, context);
        }
    }
}
