using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Framework;
using R_Tree_Framework.Utility;
using System.Runtime.InteropServices;

namespace R_Tree_Framework.Index
{
    public class LeafNodeEntry : NodeEntry
    {
        protected Int32 recordID;

        public virtual Int32 RecordID
        {
            get { return recordID; }
            protected set { recordID = value; }
        }

        public LeafNodeEntry(MinimumBoundingBox minimumBoundingBox, Int32 recordID)
        {
            MinimumBoundingBox = minimumBoundingBox;
            RecordID = recordID;
        }
        public LeafNodeEntry(Byte[] byteData)
        {
            Reconstruct(byteData, 0, byteData.Length);
        }
        public LeafNodeEntry(Byte[] byteData, Int32 offset)
        {
            Reconstruct(byteData, offset, byteData.Length);
        }
        public LeafNodeEntry(Byte[] byteData, Int32 offset, Int32 endAddress)
        {
            Reconstruct(byteData, offset, endAddress);
        }

        protected virtual void Reconstruct(Byte[] byteData, Int32 offset, Int32 endAddress)
        {
            RecordID = BitConverter.ToInt32(byteData, offset);
            offset += Marshal.SizeOf(RecordID);
            MinimumBoundingBox = new MinimumBoundingBox(byteData, offset, endAddress);
        }

        public override Byte[] GetBytes()
        {
            Int32 index = 0;
            Byte[] data = new Byte[GetSize()];
            BitConverter.GetBytes(RecordID).CopyTo(data, index);
            index += Marshal.SizeOf(RecordID);
            MinimumBoundingBox.GetBytes().CopyTo(data, index);
            return data;
        }
        public override Int32 GetSize()
        {
            return MinimumBoundingBox.GetSize() + Marshal.SizeOf(RecordID.GetType());
        }

        public static Int32 GetSize(Int32 dimension)        {

            return MinimumBoundingBox.GetSize(dimension) + Marshal.SizeOf(typeof(Int32));
        }
    }
}
