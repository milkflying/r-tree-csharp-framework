using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace R_Tree_Framework.Utility.Exceptions
{
    /// <summary>
    /// This exception is thrown when the Address class is initialized to some seed value
    /// after having previously been initialized with a seed value.  This is only a
    /// runtime warning and is intended to be a check to ensure the seed value is only
    /// initialized at an intended time.  The new seed value is persisted thus the
    /// initialization action completes prior to the error being thrown thus allowing
    /// for multiple initializations of the seed address value.
    /// </summary>
    public class AddressAlreadyInitializedWarning : AddressException
    {
        public AddressAlreadyInitializedWarning()
            : base()
        {
        }
        public AddressAlreadyInitializedWarning(String message)
            : base(message)
        {
        }
        public AddressAlreadyInitializedWarning(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public AddressAlreadyInitializedWarning(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
