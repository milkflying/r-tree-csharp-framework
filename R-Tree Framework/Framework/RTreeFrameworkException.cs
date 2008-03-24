using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Framework
{
    public class RTreeFrameworkException : Exception
    {
        public RTreeFrameworkException()
            : base()
        {
        }
        public RTreeFrameworkException(String message)
            : base(message)
        {
        }
        public RTreeFrameworkException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public RTreeFrameworkException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}


/*
 * TEMPLATE EXCEPTION
 * 
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Utility.Exceptions
{
    public class IncompatibleDimensionsException : Exception
    {
        public IncompatibleDimensionsException()
            : base()
        {
        }
        public IncompatibleDimensionsException(String message)
            : base(message)
        {
        }
        public IncompatibleDimensionsException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public IncompatibleDimensionsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
*/