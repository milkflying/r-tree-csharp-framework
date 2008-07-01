using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Utility.Exceptions;
using System.Runtime.InteropServices;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Utility
{
    /// <summary>
    /// This class represents an n-dimensional rectangle that serves as the minimum bounding rectangle
    /// of a spatial object.  For each dimension of the rectangle, a minimum value and a maximum value
    /// are stored.  Objects returned from this class that are modified will not affect the original
    /// object.  For example, changes made to the list returned from the 
    /// <see cref="R_Tree_Framework.Utility.MinimumBoundingBox.MinimumValues"/> property will not alter the minimum values
    /// stored within the MinimumBoundingBox object.  Alterations to the bounding box are not
    /// allowed after construction.
    /// </summary>
    public class MinimumBoundingBox: UtilityObject, ISavable
    {
        #region Instance Variables

        protected Int32 _dimension;
        protected List<Single> _minimumValues, _maximumValues;

        #endregion Instance Variables
        #region Properties

        /// <summary>
        /// The number of dimensions in which the minimum bounding box exists.
        /// </summary>
        public virtual Int32 Dimension
        {
            get { return _dimension; }
            protected set { _dimension = value; }
        }
        /// <summary>
        /// A Pair of lists containing the minimum and maximum value in each dimension in which the minimum bounding box exists.
        /// This returns copies of the Lists and not the actual List objects of the class.
        /// </summary>
        public virtual Pair<List<Single>, List<Single>> Extremes
        {
            get { return new Pair<List<Single>, List<Single>>(new List<Single>(_minimumValues), new List<Single>(_maximumValues)); }
        }
        /// <summary>
        /// A List of the maximum values for each dimension in which the minimum bounding box exists.
        /// This returns a copy of the List and not the actual List object of the class.
        /// </summary>
        public virtual List<Single> MaximumValues
        {
            get { return new List<Single>(_maximumValues); }
            protected set { _maximumValues = value; }
        }
        /// <summary>
        /// A List of the minimum values for each dimension in which the minimum bounding box exists.
        /// This returns a copy of the List and not the actual List object of the class.
        /// </summary>
        public virtual List<Single> MinimumValues
        {
            get { return new List<Single>(_minimumValues); }
            protected set { _minimumValues = value; }
        }
        /// <summary>
        /// A List of the maximum values for each dimension in which the minimum bounding box exists.
        /// This method is for internal class and subclass use and provides direct access to the
        /// underlying List.
        /// </summary>
        protected virtual List<Single> InternalMaximumValues
        {
            get { return _maximumValues; }
            set { _maximumValues = value; }
        }
        /// <summary>
        /// A List of the minimum values for each dimension in which the minimum bounding box exists.
        /// This method is for internal class and subclass use and provides direct access to the
        /// underlying List.
        /// </summary>
        public virtual List<Single> InternalMinimumValues
        {
            get { return _minimumValues; }
            set { _minimumValues = value; }
        }

        #endregion Properties
        #region Constructors

        /// <summary>
        /// Constructs a minimum bounding box with the minimum values in each dimension as specified
        /// in the minimumValues parameter and with the corresponding maximum values as specified
        /// in the maximumValues parameter.  Dimensions are indicated by the index into each list.
        /// The minimum value of each dimension must be less than or equal to the maximum value
        /// of the dimension.
        /// </summary>
        /// <param name="minimumValues">The minimum value for each dimension.</param>
        /// <param name="maximumValues">The maximum value for each dimension.</param>
        /// <exception cref="InvalidDimensionException">Thrown when the number of dimensions is less than 1.</exception>
        /// <exception cref="IncompatibleDimensionsException">Thrown when the number of dimensions specified by the list of minimum values does not match the number of dimensions specified by the list of maximum values.</exception>
        /// <exception cref="InvalidRectangleException">Thrown when the minimum value of a dimension is greater than the maximum value of a dimension.</exception>
        public MinimumBoundingBox(List<Single> minimumValues, List<Single> maximumValues)
        {
            if (minimumValues.Count < 1)
                throw new InvalidDimensionException(minimumValues.Count);
            if (minimumValues.Count != maximumValues.Count)
                throw new IncompatibleDimensionsException(minimumValues.Count, maximumValues.Count);
            Dimension = minimumValues.Count;
            MinimumValues = minimumValues;
            MaximumValues = maximumValues;
            for (Int32 dimensionCounter = 0; dimensionCounter < Dimension; dimensionCounter++)
                if (maximumValues[dimensionCounter].CompareTo(minimumValues[dimensionCounter]) < 0)
                    throw new InvalidRectangleException(dimensionCounter, minimumValues[dimensionCounter], maximumValues[dimensionCounter]);
        }
        /// <summary>
        /// This constructor provides a means of reconstructing a MinimumBoundingBox
        /// object that has been saved using the <see cref="MinimumBoundingBox.GetBytes"/>
        /// method from the <see cref="ISavable"/> interface.  This constructor begins reading bytes from
        /// the beginning of the buffer and stops at the end of the buffer.
        /// </summary>
        /// <param name="byteData">The byte data saved from a previous MinimumBoundingBox object</param>
        public MinimumBoundingBox(Byte[] byteData)
        {
            Reconstruct(byteData, 0, byteData.Length);
        }
        /// <summary>
        /// This constructor provides a means of reconstructing a MinimumBoundingBox
        /// object that has been saved using the <see cref="MinimumBoundingBox.GetBytes"/>
        /// method from the <see cref="ISavable"/> interface.  This constructor begins reading bytes at the
        /// specified offset value and stops at the end of the buffer.
        /// </summary>
        /// <param name="byteData">The byte data saved from a previous MinimumBoundingBox object</param>
        /// <param name="offset">The start index in the buffer</param>
        public MinimumBoundingBox(Byte[] byteData, Int32 offset)
        {
            Reconstruct(byteData, offset, byteData.Length);
        }
        /// <summary>
        /// This constructor provides a means of reconstructing a MinimumBoundingBox
        /// object that has been saved using the <see cref="MinimumBoundingBox.GetBytes"/>
        /// method from the <see cref="ISavable"/> interface.  This constructor begins reading bytes at the
        /// specified offset value and stops when it reaches the end address.  The byte indexed by the end
        /// address is not considered part of the MinimumBoundingBox saved data.
        /// </summary>
        /// <param name="byteData">The byte data saved from a previous MinimumBoundingBox object</param>
        /// <param name="offset">The start index in the buffer</param>
        /// <param name="endAddress">One past the last valid index in the buffer</param>
        public MinimumBoundingBox(Byte[] byteData, Int32 offset, Int32 endAddress)
        {
            Reconstruct(byteData, offset, endAddress);
        }


        #endregion Constructors
        #region ISavable Methods

        /// <summary>
        /// This method generates an array of bytes that can be used to regenerate
        /// the object.  This is meant to be used for saving the object and later
        /// reconstruction.
        /// </summary>
        /// <remarks>
        /// The format of the byte array is pairs of values representing the
        /// minimum and then maximum for each dimension.  For instance
        /// {minX, maxX, minY, maxY} if the object described a cartesean plane.
        /// </remarks>
        /// <returns>An array of Bytes representing the Minimum and Maximum coordinates</returns>
        public virtual Byte[] GetBytes()
        {
            Int32 coordinateSize;
            unsafe { coordinateSize = sizeof(Single); }
            Byte[] saveData = new Byte[GetSize()];
            for (Int32 i = 0, saveDataLocation = 0; i < Dimension; i++, saveDataLocation += coordinateSize)
            {
                BitConverter.GetBytes(MinimumValues[i]).CopyTo(saveData, saveDataLocation);
                saveDataLocation += coordinateSize;
                BitConverter.GetBytes(MaximumValues[i]).CopyTo(saveData, saveDataLocation);
            }
            return saveData;
            
        }
        /// <summary>
        /// This method returns the size of the object.  It calculates the size of the 
        /// generic type for this instance of the class and calculates the number of
        /// values of that type are needed for full reconstruction of the object.
        /// </summary>
        /// <returns>The size in bytes of the object.</returns>
        public virtual Int32 GetSize()
        {
            unsafe { return sizeof(Single)  * Dimension * 2;}
        }
        /// <summary>
        /// This method reconstructs a MinimumBoundingBox object based on saved
        /// byte data.
        /// </summary>
        /// <param name="byteData">The Byte[] buffer from which to reconstruct</param>
        /// <param name="offset">The start index in the buffer</param>
        /// <param name="endAddress">One past the last valid index in the buffer</param>
        protected virtual void Reconstruct(Byte[] byteData, Int32 offset, Int32 endAddress)
        {
            MinimumValues = new List<Single>();
            MaximumValues = new List<Single>();

            Int32 coordinateSize;
            unsafe { coordinateSize = sizeof(Single); }
            if (!((endAddress - offset) > 0 && (endAddress - offset) % (coordinateSize * 2) == 0))
                throw new InvalidMinimumBoundingBoxDataException();
            for (Int32 dimension = 1; offset < endAddress; dimension++)
            {
                Single minimumValue = BitConverter.ToSingle(byteData, offset), maximumValue;
                offset += coordinateSize;
                maximumValue = BitConverter.ToSingle(byteData, offset);
                offset += coordinateSize;
                if (maximumValue.CompareTo(minimumValue) < 0)
                    throw new InvalidRectangleException(dimension, minimumValue, maximumValue);
                InternalMinimumValues.Add(minimumValue);
                InternalMaximumValues.Add(maximumValue);
            }
            Dimension = InternalMinimumValues.Count;
        }
        /// <summary>
        /// Returns the size in bytes of a MinimumBoundingBox of the specified dimension
        /// </summary>
        /// <param name="dimension">The number of dimensions of a MinimumBoundingBox</param>
        /// <returns>The size of the MinimumBoundingBox in bytes.</returns>
        public static Int32 GetSize(Int32 dimension)
        {
            unsafe { return sizeof(Single) * dimension * 2; }
        }

        #endregion ISavable Methods
    }
}