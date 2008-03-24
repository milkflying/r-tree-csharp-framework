using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Exceptions;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Utility.Exceptions
{
    public class MinimumBoundingBoxException : UtilityException
    {
        public MinimumBoundingBoxException()
            : base()
        {
        }
        public MinimumBoundingBoxException(String message)
            : base(message)
        {
        }
        public MinimumBoundingBoxException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public MinimumBoundingBoxException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
