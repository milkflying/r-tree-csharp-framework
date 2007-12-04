using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class Record
    {
        protected virtual MinimumBoundingBox minimumBoundingBox;
        protected virtual Guid address;
        protected virtual Int32 recordID;

        public virtual Int32 RecordID
        {
            get { return recordID; }
            protected set { recordID = value; }
        }
        public virtual MinimumBoundingBox MinimumBoundingBox
        {
            get { return minimumBoundingBox; }
            protected set { minimumBoundingBox = value; }
        }
        public virtual Guid Address
        {
            get { return address; }
            protected set { address = value; }
        }

        public Record(Int32 recordID, MinimumBoundingBox minimumBoundingBox)
        {
            RecordID = recordID;
            Address = Guid.NewGuid();
            MinimumBoundingBox = minimumBoundingBox;
        }

        public virtual Byte[] GeneratePageData()
        {
            return new Byte[1];
        }
    }
}
