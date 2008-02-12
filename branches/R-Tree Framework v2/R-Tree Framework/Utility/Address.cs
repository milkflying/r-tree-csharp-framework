using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Utility.Exceptions;
using System.Runtime.InteropServices;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Utility
{
    public class Address : RTreeFrameworkObject, ISavable
    {
        #region Static Members 

        #region Class Variables

        protected static Int32 nextAddress;
        protected static Boolean initialized = false;

        #endregion Class Variables
        #region Properties

        public static Address NextAddress
        {
            get
            {
                if (Initialized)
                    return new Address(nextAddress++);
                else
                    throw new AddressNotInitializedException();
            }
        }
        protected static Boolean Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }

        #endregion Properties
        #region Methods

        public static void InitializeAddress(Int32 seed)
        {
            nextAddress = seed;
            if (Initialized)
                throw new AddressAlreadyInitializedWarning();
            Initialized = true;
        }
        public static Address ReconstructAddress(Byte[] data)
        {
            return new Address(data);
        }
        public static Address ReconstructAddress(Byte[] data, Int32 offset)
        {
            return new Address(data, offset);
        }

        #endregion Methods

        #endregion Static Members
        #region Instance Members

        #region Instance Variables

        protected Int32 memoryAddress;

        #endregion Instance Variables
        #region Properties

        protected Int32 MemoryAddress
        {
            get { return memoryAddress; }
            set { memoryAddress = value; }
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
