using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Utility.Exceptions;
using System.Runtime.InteropServices;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Utility
{
    public class Address : UtilityObject, ISavable
    {
        #region Static Members 

        #region Class Variables

        protected static Int32 _nextAddress;
        protected static Boolean _initialized = false;
        protected static Address _nullAddress = new Address(0);

        #endregion Class Variables
        #region Properties

        /// <summary>
        /// Provides an address that can be used for an unitialized or zero value.
        /// </summary>
        public static Address NullAddress
        {
            get { return _nullAddress; }
        }
        /// <summary>
        /// Generates an Adderss object with the next sequential address value.
        /// </summary>
        public static Address NextAddress
        {
            get
            {
                if (Initialized)
                    return new Address(_nextAddress++);
                else
                    throw new AddressNotInitializedException();
            }
        }
        /// <summary>
        /// Returns wether the Address class has been initialized with a seed value.
        /// </summary>
        protected static Boolean Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        #endregion Properties
        #region Methods

        /// <summary>
        /// Initializes the Address class to the provided seed value.
        /// </summary>
        /// <param name="seed">The starting address value</param>
        public static void InitializeAddress(Int32 seed)
        {
            if (seed > 0)
            {
                nextAddress = seed;
                if (Initialized)
                    throw new AddressAlreadyInitializedWarning();
                Initialized = true;
            }
            else
                throw new InvalidAddressSeedException();
        }
        /// <summary>
        /// Reconstructs an Address value from a given raw data byte array.
        /// </summary>
        /// <param name="data">The raw data from which to resconstruct an Address</param>
        /// <returns>The recontructed Address object</returns>
        public static Address ReconstructAddress(Byte[] data)
        {
            return new Address(data);
        }
        /// <summary>
        /// Reconstructs an Address value from a given raw data byte array starting at a specified index.  
        /// </summary>
        /// <param name="data">The raw data from which to resconstruct an Address</param>
        /// <returns>The recontructed Address object</returns>
        public static Address ReconstructAddress(Byte[] data, Int32 offset)
        {
            return new Address(data, offset);
        }

        #endregion Methods

        #endregion Static Members
        #region Instance Members

        #region Instance Variables

        protected Int32 _memoryAddress;

        #endregion Instance Variables
        #region Properties

        protected Int32 MemoryAddress
        {
            get { return _memoryAddress; }
            set { _memoryAddress = value; }
        }

        #endregion Properties
        #region Constructors

        protected Address(Int32 address)
        {
            MemoryAddress = address;
        }
        protected Address(Byte[] data)
        {
            MemoryAddress = BitConverter.ToInt32(data, 0);
        }
        protected Address(Byte[] data, Int32 offset)
        {
            MemoryAddress = BitConverter.ToInt32(data, offset);
        }

        #endregion Constructors
        #region ISavable Methods

        /// <summary>
        /// This method generates an array of bytes that can be used to regenerate
        /// the object.  This is meant to be used for saving the object and later
        /// reconstruction.
        /// </summary>
        /// <returns>An array of Bytes representing the Address internal value</returns>
        public virtual Byte[] GetBytes()
        {
            return BitConverter.GetBytes(MemoryAddress);
        }
        /// <summary>
        /// This method returns the size of the underlying value type.
        /// </summary>
        /// <returns>The size in bytes of the object.</returns>
        public virtual unsafe Int32 GetSize()
        {
            return Marshal.SizeOf(typeof(Int32));
        }

        #endregion ISavable Methods

        #endregion Instance Members
    }
}
