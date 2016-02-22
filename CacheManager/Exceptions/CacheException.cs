using Artisan.Tools.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace Artisan.Tools.CacheManager.Exceptions
{
    [Serializable]
    public class CacheException : AppException
    {
        public CacheException(string message, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0) : 
            base( message, sourceFilePath, methodName, sourceLineNumber) { 
            
        }

        public CacheException(Exception innerException, string message, [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0) : 
            base( innerException, message, sourceFilePath, methodName, sourceLineNumber) {   }

        protected CacheException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
