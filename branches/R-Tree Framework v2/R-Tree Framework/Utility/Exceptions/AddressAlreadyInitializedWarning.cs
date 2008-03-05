using System;
using System.Collections.Generic;
using System.Text;

namespace R_Tree_Framework.Utility.Exceptions
{
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
