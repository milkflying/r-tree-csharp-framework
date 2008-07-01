using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Utility.Exceptions
{
    /// <summary>
    /// This exception is thrown when a negative seed value is given to the Address class
    /// for initialization.  Address values must be positive values.
    /// </summary>
    public class InvalidAddressSeedException : AddressException
    {
        public InvalidAddressSeedException()
            : base()
        {
        }
        public InvalidAddressSeedException(String message)
            : base(message)
        {
        }
        public InvalidAddressSeedException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public InvalidAddressSeedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
