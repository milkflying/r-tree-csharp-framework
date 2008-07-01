using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Utility.Exceptions
{
    /// <summary>
    /// This exception is thrown when the data buffer given to MinimumBoundingBox can not
    /// be used to generate a MinimumBoundingBox object.  This is caused when the length of
    /// the buffer does not correspond to some multiple of twice the size of the underlying
    /// data type of MinimumBoundingBox.  Under the current implementation this requires
    /// the buffer length to be a multiple of twice the size of a Single. In other words,
    /// Buffer.Length % (2 * sizeof(CoordinateDataType)) == 0.
    /// </summary>
    public class InvalidMinimumBoundingBoxDataException : MinimumBoundingBoxException
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