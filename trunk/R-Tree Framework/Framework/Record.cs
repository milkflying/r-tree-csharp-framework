using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class Record : PageData
    {
        protected MinimumBoundingBox minimumBoundingBox;
        protected Address address;
        protected Int32 recordID;

        public virtual Int32 RecordID
        {
            get { return recordID; }
            protected set { recordID = value; }
        }
        public virtual MinimumBoundingBox BoundingBox
        {
            get { return minimumBoundingBox; }
            protected set { minimumBoundingBox = value; }
        }
        public virtual Address Address
        {
            get { return address; }
            protected set { address = value; }
        }

        public Record(Int32 recordID, MinimumBoundingBox minimumBoundingBox)
        {
            RecordID = recordID;
            Address = Address.NewAddress();
            BoundingBox = minimumBoundingBox;
        }
        public Record(Address Address, Byte[] data)
        {
            BoundingBox = new MinimumBoundingBox(
                BitConverter.ToSingle(data, 0),
                BitConverter.ToSingle(data, 8),
                BitConverter.ToSingle(data, 16),
                BitConverter.ToSingle(data, 24));
            RecordID = BitConverter.ToInt32(data, 32);
        }

        public virtual Byte[] GeneratePageData()
        {
            Byte[] data = new Byte[Constants.RECORD_SIZE];
            BitConverter.GetBytes(BoundingBox.MinX).CopyTo(data, 0);
            BitConverter.GetBytes(BoundingBox.MinY).CopyTo(data, 8);
            BitConverter.GetBytes(BoundingBox.MaxX).CopyTo(data, 16);
            BitConverter.GetBytes(BoundingBox.MaxY).CopyTo(data, 24);
            BitConverter.GetBytes(RecordID).CopyTo(data, 32);
            return data;
        }
    }
}
