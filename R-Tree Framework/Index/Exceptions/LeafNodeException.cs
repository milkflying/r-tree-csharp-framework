using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Index.Exceptions
{
    public abstract class LeafNodeException : NodeException
    {
                public LeafNodeException()
            : base()
        {
        }
        public LeafNodeException(String message)
            : base(message)
        {
        }
        public LeafNodeException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public LeafNodeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
