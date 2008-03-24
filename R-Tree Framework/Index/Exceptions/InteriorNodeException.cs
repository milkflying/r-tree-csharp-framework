using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Index.Exceptions
{
    public abstract class InteriorNodeException : NodeException
    {
                public InteriorNodeException()
            : base()
        {
        }
        public InteriorNodeException(String message)
            : base(message)
        {
        }
        public InteriorNodeException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public InteriorNodeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
