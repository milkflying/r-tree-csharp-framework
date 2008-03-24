using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Exceptions
{
    public class UtilityException : RTreeFrameworkException
    {
        public UtilityException()
            : base()
        {
        }
        public UtilityException(String message)
            : base(message)
        {
        }
        public UtilityException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public UtilityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
