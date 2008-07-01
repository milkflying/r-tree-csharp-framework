using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Utility.Exceptions
{
    /// <summary>
    /// This exception is thrown if the Address class has not been initialized with a seed value
    /// and the NextAddress property is used to generate a new Address.
    /// </summary>
    public class AddressNotInitializedException : AddressException
    {
                public AddressNotInitializedException()
            : base()
        {
        }
        public AddressNotInitializedException(String message)
            : base(message)
        {
        }
        public AddressNotInitializedException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public AddressNotInitializedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
