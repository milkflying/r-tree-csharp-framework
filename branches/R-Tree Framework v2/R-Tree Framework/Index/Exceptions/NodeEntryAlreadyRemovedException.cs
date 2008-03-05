using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Index.Exceptions
{
    public class NodeEntryAlreadyRemovedException : NodeException
    {
                public NodeEntryAlreadyRemovedException()
            : base()
        {
        }
        public NodeEntryAlreadyRemovedException(String message)
            : base(message)
        {
        }
        public NodeEntryAlreadyRemovedException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public NodeEntryAlreadyRemovedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
