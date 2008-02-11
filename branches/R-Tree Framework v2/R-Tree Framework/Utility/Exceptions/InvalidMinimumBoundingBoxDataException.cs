using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Utility.Exceptions
{
    public class InvalidMinimumBoundingBoxDataException : Exception
    {
        public InvalidMinimumBoundingBoxDataException()
            : base()
        {
        }
        public InvalidMinimumBoundingBoxDataException(String message)
            : base(message)
        {
        }
        public InvalidMinimumBoundingBoxDataException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public InvalidMinimumBoundingBoxDataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}