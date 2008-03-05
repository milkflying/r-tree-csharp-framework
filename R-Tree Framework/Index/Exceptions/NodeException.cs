using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Exceptions;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Index.Exceptions
{
    public abstract class NodeException : IndexException
    {
                public NodeException()
            : base()
        {
        }
        public NodeException(String message)
            : base(message)
        {
        }
        public NodeException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public NodeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
