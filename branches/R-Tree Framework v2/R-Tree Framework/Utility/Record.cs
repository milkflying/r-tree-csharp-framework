using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Utility
{
    public class Record : UtilityObject
    {
        protected Int32 recordID;
        protected Byte[] data;

        public virtual Int32 RecordID
        {
            get { return recordID; }
            protected set { recordID = value; }
        }
        public virtual Byte[] Data
        {
            get { return data; }
            protected set { data = value; }
        }

        public Record(Int32 recordID)
        {
            RecordID = recordID;
            Data = new Byte[0];
        }
        public Record(Int32 recordID, Byte[] data)
        {
            RecordID = recordID;
            Data = data;
        }

        public void SetData(Byte[] Data)
        {
            Data = data;
        }
    }
}
