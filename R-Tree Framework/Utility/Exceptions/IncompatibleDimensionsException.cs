using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Utility.Exceptions
{
    public class IncompatibleDimensionsException : Exception
    {
        protected Int32 minimumsDimension, maximumsDimension;

        public virtual Int32 MaximumsDimension
        {
            get { return maximumsDimension; }
            protected set { maximumsDimension = value; }
        }
        public virtual Int32 MinimumsDimension
        {
            get { return minimumsDimension; }
            protected set { minimumsDimension = value; }
        }
        
        public IncompatibleDimensionsException(Int32 minimumsDimension, Int32 maximumsDimension)
            : base()
        {
            SetDimensionInformation(minimumsDimension, maximumsDimension);
        }
        public IncompatibleDimensionsException(Int32 minimumsDimension, Int32 maximumsDimension, String message)
            : base(SetMessage(message, minimumsDimension, maximumsDimension))
        {
            SetDimensionInformation(minimumsDimension, maximumsDimension);
        }
        public IncompatibleDimensionsException(Int32 minimumsDimension, Int32 maximumsDimension, String message, Exception innerException)
            : base(SetMessage(message, minimumsDimension, maximumsDimension), innerException)
        {
            SetDimensionInformation(minimumsDimension, maximumsDimension);
        }
        public IncompatibleDimensionsException(Int32 minimumsDimension, Int32 maximumsDimension, SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            SetDimensionInformation(minimumsDimension, maximumsDimension);
        }

        protected virtual void SetDimensionInformation(Int32 minimumsDimension, Int32 maximumsDimension)
        {
            MinimumsDimension = minimumsDimension;
            MaximumsDimension = maximumsDimension;
        }

        private static String SetMessage(String message, Int32 minimumsDimension, Int32 maximumsDimension)
        {
            return message + String.Format("{0}The number of minimum values did not match the number of maximum values.{0}Minimum Values Count: {1}{0}Maximum Values Count: {2}", Environment.NewLine, minimumsDimension, maximumsDimension);
        }
    }
}


/*
 * TEMPLATE EXCEPTION
 * 
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Utility.Exceptions
{
    public class IncompatibleDimensionsException : Exception
    {
        public IncompatibleDimensionsException()
            : base()
        {
        }
        public IncompatibleDimensionsException(String message)
            : base(message)
        {
        }
        public IncompatibleDimensionsException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        public IncompatibleDimensionsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
*/