using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Exceptions;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Utility.Exceptions
{
    public class AddressException : UtilityException
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