using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Exceptions
{
    public class CacheManagerException : RTreeFrameworkException
    {
        public CacheManagerException()
            : base()
        {
        }
        public CacheManagerException(String message)
            : base(message)
        {
        }
        public CacheManagerException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public CacheManagerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
