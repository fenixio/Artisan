using System;
using System.Runtime.CompilerServices;
using Artisan.Tools.Reflector;
using System.Text;

namespace Artisan.Tools.Logger
{
    public static class LogExtension
    {

        public static void WriteLog(this object o, Level level, string message, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Log.WriteToLog(level, null, message, sourceFilePath, methodName, sourceLineNumber);
        }

        public static void WriteLog(this object o, Level level, string format, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "",  [CallerLineNumber] int sourceLineNumber = 0, params object[] args)
        {
            Log.WriteToLog(level, null, string.Format(format, args), sourceFilePath, methodName, sourceLineNumber);
        }

        public static void WriteLog(this object o, Level level, Exception ex, string format, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0, params object[] args)
        {
            Log.WriteToLog(level, ex, string.Format(format, args), sourceFilePath, methodName, sourceLineNumber);
        }

        public static void WriteLog(this object o, Level level, Exception ex, string message, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Log.WriteToLog(level, ex, message, sourceFilePath, methodName, sourceLineNumber);
        }

        public static void DumpToLog(this object o, Level level)
        {
            if (o != null)
            {
                Type otype = o.GetType();

                var serializer = TextSerializerBuilder.Instance.Create(otype);
                StringBuilder buffer = new StringBuilder();
                buffer.AppendFormat("{0}:", otype.Name);
                serializer.Serialize(o, buffer);
                Log.Print(level, buffer.ToString());
            }

        }

        public static void DumpToLog(this object o, Level level, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (o != null)
            {
                Type otype = o.GetType();

                var serializer = TextSerializerBuilder.Instance.Create(otype);
                StringBuilder buffer = new StringBuilder();
                buffer.AppendFormat("{0}:", otype.Name);
                serializer.Serialize(o, buffer);
                Log.WriteToLog(level, null, buffer.ToString(), sourceFilePath, methodName, sourceLineNumber);
            }
        }

    }
}
