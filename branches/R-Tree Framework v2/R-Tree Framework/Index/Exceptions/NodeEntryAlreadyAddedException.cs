using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Index.Exceptions
{
    public class NodeEntryAlreadyAddedException : NodeException
    {
                public NodeEntryAlreadyAddedException()
            : base()
        {
        }
        public NodeEntryAlreadyAddedException(String message)
            : base(message)
        {
        }
        public NodeEntryAlreadyAddedException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public NodeEntryAlreadyAddedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
