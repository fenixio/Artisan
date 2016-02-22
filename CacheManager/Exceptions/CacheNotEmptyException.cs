using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace Artisan.Tools.CacheManager.Exceptions
{
    [Serializable]
    public class CacheNotEmptyException : CacheException
    {
        public CacheNotEmptyException( [CallerFilePath] string sourceFilePath = "", [CallerMemberName]string methodName = "", [CallerLineNumber] int sourceLineNumber = 0) : base("Cache is not empty. Cache must be empty before set it up", sourceFilePath, methodName, sourceLineNumber) { }

        protected CacheNotEmptyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}