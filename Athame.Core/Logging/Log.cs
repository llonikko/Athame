using System;
using System.Collections.Generic;

namespace Athame.Core.Logging
{
    public static class Log
    {
        private static readonly Dictionary<string, ILogger> loggers = new Dictionary<string, ILogger>();

        static Log()
        {
            Filter = Level.Debug;
        }

        public static void AddLogger(string name, ILogger instance)
        {
            loggers[name] = instance;
        }

        public static void RemoveLogger(string name)
        {
            loggers.Remove(name);
        }

        public static ILogger GetLogger(string name)
        {
            return loggers[name];
        }

        public static T GetLogger<T>(string name) where T : ILogger
        {
            return (T) GetLogger(name);
        }

        public static Level Filter { get; set; }

        public static void Write(Level level, string tag, string message)
        {
            if (level < Filter)
            {
                return;
            }
            foreach (var logger in loggers)
            {
                logger.Value.Write(level, tag, message);
            }
        }

        public static void WriteException(Level level, string tag, Exception ex, string message = null)
        {
            var messageToWrite = (message ?? "Unhandled exception") + "\n";
            if (ex != null)
            {
                messageToWrite += $"{ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}";
            }
            else
            {
                messageToWrite += "!!! Exception passed is null !!!";
            }
            Write(level, tag, messageToWrite);
        }

        public static void Debug(string tag, string message)
        {
            Write(Level.Debug, tag, message);
        }

        public static void Info(string tag, string message)
        {
            Write(Level.Info, tag, message);
        }

        public static void Warning(string tag, string message)
        {
            Write(Level.Warning, tag, message);
        }

        public static void Error(string tag, string message)
        {
            Write(Level.Error, tag, message);
        }

        public static void Fatal(string tag, string message)
        {
            Write(Level.Fatal, tag, message);
        }
    }
}
