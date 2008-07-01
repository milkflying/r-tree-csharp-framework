using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Exceptions;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Utility.Exceptions
{
    /// <summary>
    /// This class is the base class for all Address Exceptions that are generated.
    /// </summary>
    public abstract class AddressException : UtilityException
    {
        public AddressException()
            : base()
        {
        }
        public AddressException(String message)
            : base(message)
        {
        }
        public AddressException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public AddressException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
