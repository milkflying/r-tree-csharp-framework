using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace R_Tree_Framework.Utility.Exceptions
{
    /// <summary>
    /// This exception is thrown when a minimum value in a dimension of a MinimumBoundingBox
    /// is greater than the maximum value for that dimension.
    /// </summary>
    public class InvalidRectangleException : MinimumBoundingBoxException
    {
        protected Int32 dimension;
        protected Single minimumValue, maximumValue;

        public virtual Int32 Dimension
        {
            get { return dimension; }
            protected set { dimension = value; }
        }
        public virtual Single MinimumValue
        {
            get { return minimumValue; }
            protected set { minimumValue = value; }
        }
        public virtual Single MaximumValue
        {
            get { return maximumValue; }
            protected set { maximumValue = value; }
        }

        public InvalidRectangleException(Int32 dimension, Single minimumValue, Single maximumValue)
            : base()
        {
            SetRectangleInformation(dimension, minimumValue, maximumValue);
        }
        public InvalidRectangleException(Int32 dimension, Single minimumValue, Single maximumValue, String message)
            : base(SetMessage(message, dimension, minimumValue, maximumValue))
        {
            SetRectangleInformation(dimension, minimumValue, maximumValue);
        }
        public InvalidRectangleException(Int32 dimension, Single minimumValue, Single maximumValue, String message, Exception innerException)
            : base(SetMessage(message, dimension, minimumValue, maximumValue), innerException)
        {
            SetRectangleInformation(dimension, minimumValue, maximumValue);
        }
        public InvalidRectangleException(Int32 dimension, Single minimumValue, Single maximumValue, SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            SetRectangleInformation(dimension, minimumValue, maximumValue);
        }

        protected virtual void SetRectangleInformation(Int32 dimension, Single minimumValue, Single maximumValue)
        {
            Dimension = dimension;
            MinimumValue = minimumValue;
            MaximumValue = maximumValue;
        }
        private static String SetMessage(String message, Int32 dimension, Single minimumValue, Single maximumValue)
        {
            return message + String.Format("{0}Minimum values of a dimension must be less than or equal to the maximum value of the same dimension.  The minimum value for dimension {1} is greater than the maximum value.{0}Dimension: {1}{0}Minimum Value: {2}{0}Maximum Value: {3}", Environment.NewLine, dimension, minimumValue, maximumValue);
        }
    }
}