using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Framework;
using R_Tree_Framework.Utility.Exceptions;

namespace R_Tree_Framework.Utility
{
    public class Page : UtilityObject, IAddressable, ISavable
    {
        #region Instance Variables

        protected Int32 _pageSize;
        protected Byte[] _data;
        protected Address _address;

        #endregion Instance Variables
        #region Properties

        /// <summary>
        /// The size of the Page.  The size of the actual meaningful data
        /// can be less that PageSize but the Data property will always return
        /// a Byte[] of PageSize.
        /// </summary>
        public virtual Int32 PageSize
        {
            get { return _pageSize; }
            protected set { _pageSize = value; }
        }
        /// <summary>
        /// The byte data that the Page encapsulates.
        /// </summary>
        protected virtual Byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        /// The memory address of the page
        /// </summary>
        public virtual Address MemoryAddress
        {
            get { return _address; }
            protected set { _address = value; }
        }

        #endregion Properties
        #region Construcors

        /// <summary>
        /// Initializes a Page object to encapsulate pageSize of data.
        /// </summary>
        /// <param name="pageSize">The size of the page's data buffer</param>
        public Page(Int32 pageSize)
        {
            PageSize = pageSize;
            Data = new Byte[PageSize];
            MemoryAddress = Address.NextAddress;
        }
        /// <summary>
        /// Initializes a Page object to encapsulate pageSize of data.
        /// </summary>
        /// <param name="pageSize">The size of the page's data buffer</param>
        /// <param name="pageAddress">The memory address of the page</param>
        public Page(Int32 pageSize, Address pageAddress)
        {
            PageSize = pageSize;
            Data = new Byte[PageSize];
            MemoryAddress = pageAddress;
        }
        /// <summary>
        /// Initializes a Page object to encapsulate pageSize of data.
        /// </summary>
        /// <param name="data">The raw data the page encapsulates</param>
        /// <param name="pageAddress">The memory address of the page</param>
        public Page(Byte[] data, Address pageAddress)
        {
            PageSize = data.Length;
            Data = data;
            MemoryAddress = pageAddress;
        }

        #endregion Construcors
        #region Methods

        /// <summary>
        /// Returns the segment of the Page's data determined by the index and size
        /// </summary>
        /// <param name="index">The index of the Page's data to begin retriving information</param>
        /// <param name="size">The length of data to retrieve</param>
        /// <returns>The segment of Page data starting at index with length size</returns>
        public virtual Byte[] GetData(Int32 index, Int32 size)
        {
            Byte[] buffer = new Byte[size];
            for (Int32 i = index, j = 0; i < index + size; i++, j++)
                buffer[j] = Data[i];
            return buffer;
        }
        /// <summary>
        /// Encapsilates the ISavable object in a page.  The length of data
        /// returned by the ISavable.GetBytes() method must be less than or equal
        /// to the PageSize.  If the data is smaller than the PageSize, excess
        /// bytes in the page will remain unchanged and NOT zero-ed out.
        /// Data is stored starting at the zero index of the page.
        /// </summary>
        /// <param name="data">The object to encapsulate</param>
        public virtual void SetData(ISavable data)
        {
            SetData(data.GetBytes());
        }
        /// <summary>
        /// Sets the Page's data to the provided data.  The length of the data
        /// must be less than or equal to the PageSize.  If the data is smaller
        /// than the PageSize, excess bytes in the page will remain unchanged
        /// and NOT zero-ed out.  Data is stored starting at the zero index of 
        /// the page.
        /// </summary>
        /// <param name="data">The data to encapsulate</param>
        public virtual void SetData(Byte[] data)
        {
            SetData(data, 0);
        }
        /// <summary>
        /// Sets the Page's data to the provided data.  The length of the data
        /// must be less than or equal to the PageSize.  If the data is smaller
        /// than the PageSize, excess bytes in the page will remain unchanged
        /// and NOT zero-ed out.  Data is stored starting at the specified index
        /// of the page.
        /// </summary>
        /// <param name="data">The data to encapsulate</param>
        /// <param name="index">The index to start storing the data</param>
        public virtual void SetData(Byte[] data, Int32 index)
        {
            if (data.Length + index > PageSize)
                throw new PageSizeTooSmallException(PageSize, data.Length + index);
            if (data.Length == PageSize)
                Data = data;
            else
            {
                data.CopyTo(Data, index);
            }

        }
        /// <summary>
        /// Returns the bytes of the page for saving.  This does not persist the address
        /// of the page.  The address must be persisted seperately if needed.
        /// </summary>
        /// <returns>The data that the page encapsulates</returns>
        public virtual Byte[] GetBytes()
        {
            return Data;
        }
        /// <summary>
        /// Returns the size of the data the page encapsulates
        /// </summary>
        /// <returns></returns>
        public virtual Int32 GetSize()
        {
            return PageSize;
        }

        #endregion Methods
    }
}
