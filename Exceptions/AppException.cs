using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artisan.Tools.Logger;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Artisan.Tools.Exceptions
{
    public class AppException : Exception
    {
        protected Level LogLevel;
        protected string LogMessage;
        protected object[] Args;
        protected string SourceFilePath;
        protected string MethodName;
        protected int SourceLineNumber;

        public AppException(string message, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0) : this(Level.Error, null, message, sourceFilePath, methodName, sourceLineNumber)
        {
        }

        public AppException(Exception ex, string message, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0) : this(Level.Error, ex, message, sourceFilePath, methodName, sourceLineNumber)
        {

        }

        public AppException(Exception ex, string format, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0, params object[] args) : this(Level.Error, ex, string.Format(format, args), sourceFilePath, methodName, sourceLineNumber)
        {

        }

        public AppException(Level level, string message, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0) : this(level, null, message, sourceFilePath, methodName, sourceLineNumber)
        {
        }

        public AppException(Level level, Exception ex, string message, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0) : base(message, ex)
        {
            this.LogLevel   = level;
            this.LogMessage = "An error has ocurred: {0}";
            this.Args       = new[] { message };
            this.SourceFilePath = sourceFilePath;
            this.MethodName = methodName;
            this.SourceLineNumber = sourceLineNumber;
            this.WriteToLog();


        }

        public AppException(Level level, Exception ex, string format, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0, params object[] args) : this(level, ex, string.Format(format, args), sourceFilePath, methodName, sourceLineNumber)
        {

        }

        protected AppException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        public virtual void WriteToLog()
        {
            Log.Write(LogLevel, InnerException, LogMessage, SourceFilePath, MethodName, SourceLineNumber, Args);
        }

    }
}
