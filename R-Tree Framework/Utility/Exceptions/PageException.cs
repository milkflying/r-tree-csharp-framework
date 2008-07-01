using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Exceptions;
using System.Runtime.Serialization;


namespace R_Tree_Framework.Utility.Exceptions
{
    /// <summary>
    /// This is the base class of all Page based exceptions.
    /// </summary>
    public abstract class PageException : UtilityException
    {
        public PageException()
            : base()
        {
        }
        public PageException(String message)
            : base(message)
        {
        }
        public PageException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public PageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
