// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace iSynaptic.Commons.Diagnostics
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
