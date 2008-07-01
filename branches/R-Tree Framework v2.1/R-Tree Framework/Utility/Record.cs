using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Utility
{
    /// <summary>
    /// The Record class represents a record entry in a database.
    /// </summary>
    public class Record : UtilityObject
    {
        #region Instance Variables

        protected Int32 _recordID;
        protected Byte[] _data;

        #endregion
        #region Properties

        /// <summary>
        /// The ID of the record
        /// </summary>
        public virtual Int32 RecordID
        {
            get { return _recordID; }
            protected set { _recordID = value; }
        }
        /// <summary>
        /// The raw data of the record
        /// </summary>
        public virtual Byte[] Data
        {
            get { return _data; }
            protected set { _data = value; }
        }

        #endregion
        #region Constructors

        /// <summary>
        /// Constructs a record with the specified ID without any data.
        /// </summary>
        /// <param name="recordID">The ID of the record</param>
        public Record(Int32 recordID)
        {
            RecordID = recordID;
            Data = new Byte[0];
        }
        /// <summary>
        /// Constructs a record with the sepcified ID and raw data
        /// </summary>
        /// <param name="recordID">The ID of the record</param>
        /// <param name="data">The raw data the record object represents</param>
        public Record(Int32 recordID, Byte[] data)
        {
            RecordID = recordID;
            Data = data;
        }

        #endregion
        #region Public Methods

        /// <summary>
        /// Sets the data to the specified raw data.
        /// </summary>
        /// <param name="data">The new data of the record</param>
        public void SetData(Byte[] data)
        {
            Data = data;
        }

        #endregion
    }
}
