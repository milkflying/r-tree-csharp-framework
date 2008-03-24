using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Exceptions
{
    public class IndexException : RTreeFrameworkException
    {
        public IndexException()
            : base()
        {
        }
        public IndexException(String message)
            : base(message)
        {
        }
        public IndexException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public IndexException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
