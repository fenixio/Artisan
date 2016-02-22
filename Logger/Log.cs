using System;
using System.IO;
using System.Runtime.CompilerServices;

using log4net;
using System.Collections.Generic;

namespace Artisan.Tools.Logger
{
    public static class Log
    {
        private static Dictionary<string, ILog> loggers = new Dictionary<string, ILog>();

        public static void Write(Level level, string message, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            WriteToLog(level, null, message, sourceFilePath, methodName, sourceLineNumber);
        }

        public static void Write(Level level, string format, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0, params object[] args)
        {
            WriteToLog(level, null, string.Format(format, args), sourceFilePath, methodName, sourceLineNumber);
        }

        public static void Write(Level level, Exception ex,  string format, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0, params object[] args)
        {
            WriteToLog(level, ex, string.Format(format, args), sourceFilePath, methodName, sourceLineNumber);
        }

        public static void Write(Level level, Exception ex, string message, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            WriteToLog(level, ex, message, sourceFilePath, methodName, sourceLineNumber);
        }


        public static void Print(Level level, string message)
        {
            PrintToLog(level, null, message);
        }

        public static void Print(Level level, string format, params object[] args)
        {
            PrintToLog(level, null, string.Format(format, args));
        }

        public static void Write(Level level, Exception ex, string message)
        {
            PrintToLog(level, ex, message );
        }

        public static void Write(Level level, Exception ex, string format, params object[] args)
        {
            PrintToLog(level, ex, string.Format(format, args) );
        }


        public static LogConfig Config { get; } = new LogConfig();

        internal static void WriteToLog(Level level, Exception ex, string message, string sourceFilePath, string methodName, int sourceLineNumber)
        {
            string file = Path.GetFileName(sourceFilePath);
            string msg = string.Format("{0}[{2}]->{1}(): {3}", file, methodName, sourceLineNumber, message);
            ILog log = GetLogger(file);

            if (ex != null)
                log.Logger.Log(typeof(LogExtension), Get4NetLevel(level), msg, ex);
            else
                log.Logger.Log(typeof(LogExtension), Get4NetLevel(level), msg, null);
        }

        internal static void PrintToLog(Level level, Exception ex, string message)
        {
            
            ILog log = GetLogger("Common");

            if (ex != null)
                log.Logger.Log(typeof(LogExtension), Get4NetLevel(level), message, ex);
            else
                log.Logger.Log(typeof(LogExtension), Get4NetLevel(level), message, null);
        }

        internal static log4net.Core.Level Get4NetLevel(Level level)
        {
            log4net.Core.Level ret;
            switch (level)
            {
                case Level.Debug:
                    ret = log4net.Core.Level.Debug;
                    break;

                case Level.Error:
                    ret = log4net.Core.Level.Error;
                    break;

                case Level.Fatal:
                    ret = log4net.Core.Level.Fatal;
                    break;

                case Level.Info:
                    ret = log4net.Core.Level.Info;
                    break;

                case Level.Warn:
                    ret = log4net.Core.Level.Warn;
                    break;

                default:
                    ret = log4net.Core.Level.Verbose;
                    break;
            }
            return ret;
        }

        private static ILog GetLogger(string logName)
        {
            ILog log;
            if (!loggers.TryGetValue(logName, out log))
            {
                log = LogManager.GetLogger("Common");
                loggers[logName] = log;
            }
            return log;
        }
    }
}