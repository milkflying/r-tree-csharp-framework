using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Utility.Exceptions
{
    /// <summary>
    /// This exception is used when the number of dimensions specified for
    /// the minimum values of a MinimumBoundingBox is less than 1.  A
    /// MinimumBoundingBox must exist in at least 1 dimension.  This
    /// exception is not thrown for the number of maximum values
    /// as an <see cref="IncompatibleDimensionException"/> is generated
    /// with the valid dimension of the minimum values.
    /// </summary>
    public class InvalidDimensionException : MinimumBoundingBoxException
    {
        protected Int32 dimensionCount;

        public virtual Int32 DimensionCount
        {
            get { return dimensionCount; }
            protected set { dimensionCount = value; }
        }

        public InvalidDimensionException(Int32 dimensionCount)
            : base()
        {
            SetDimensionInformation(dimensionCount);
        }
        public InvalidDimensionException(Int32 dimensionCount, String message)
            : base(SetMessage(message, dimensionCount))
        {
            SetDimensionInformation(dimensionCount);
        }
        public InvalidDimensionException(Int32 dimensionCount, String message, Exception innerException)
            : base(SetMessage(message, dimensionCount), innerException)
        {
            SetDimensionInformation(dimensionCount);
        }
        public InvalidDimensionException(Int32 dimensionCount, SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            SetDimensionInformation(dimensionCount);
        }

        protected virtual void SetDimensionInformation(Int32 dimensionCount)
        {
            DimensionCount = dimensionCount;
        }

        private static String SetMessage(String message, Int32 dimensionCount)
        {
            return message + String.Format("{0}The number of dimensions must be a positive integer.{0}Dimension Count: {1}", Environment.NewLine, dimensionCount);
        }
    }
}
