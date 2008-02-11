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
            set { minimumBoundingBox = value; }
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
        public Record(Address address, Byte[] data)
        {
            Address = address;
            BoundingBox = new MinimumBoundingBox(
                BitConverter.ToSingle(data, 1),
                BitConverter.ToSingle(data, 5),
                BitConverter.ToSingle(data, 9),
                BitConverter.ToSingle(data, 13));
            RecordID = BitConverter.ToInt32(data, 17);
        }

        public virtual Byte[] GeneratePageData()
        {
            Byte[] data = new Byte[Constants.RECORD_SIZE];
            data[0] = (Byte)PageDataType.Record;
            BitConverter.GetBytes(BoundingBox.MinX).CopyTo(data, 1);
            BitConverter.GetBytes(BoundingBox.MinY).CopyTo(data, 5);
            BitConverter.GetBytes(BoundingBox.MaxX).CopyTo(data, 9);
            BitConverter.GetBytes(BoundingBox.MaxY).CopyTo(data, 13);
            BitConverter.GetBytes(RecordID).CopyTo(data, 17);
            return data;
        }
    }
}
