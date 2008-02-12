using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Exceptions
{
    public class QueryException : RTreeFrameworkException
    {
        public QueryException()
            : base()
        {
        }
        public QueryException(String message)
            : base(message)
        {
        }
        public QueryException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public QueryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
