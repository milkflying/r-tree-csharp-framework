using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Framework;
using R_Tree_Framework.Utility;
using System.Runtime.InteropServices;

namespace R_Tree_Framework.Index
{
    public class InteriorNodeEntry<CoordinateType> : NodeEntry<CoordinateType> where CoordinateType : struct, IComparable
    {
        protected Address childNode;

        public virtual Address ChildNode
        {
            get { return childNode; }
            protected set { childNode = value; }
        }

        public InteriorNodeEntry(MinimumBoundingBox<CoordinateType> minimumBoundingBox, Address childNode)
        {
            MinimumBoundingBox = minimumBoundingBox;
            ChildNode = childNode;
        }
        public InteriorNodeEntry(Byte[] byteData)
        {
            Reconstruct(byteData, 0, byteData.Length);
        }
        public InteriorNodeEntry(Byte[] byteData, Int32 offset)
        {
            Reconstruct(byteData, offset, byteData.Length);
        }
        public InteriorNodeEntry(Byte[] byteData, Int32 offset, Int32 endAddress)
        {
            Reconstruct(byteData, offset, endAddress);
        }

        protected virtual void Reconstruct(Byte[] byteData, Int32 offset, Int32 endAddress)
        {
            ChildNode = Address.ReconstructAddress(byteData, offset);
            offset += Marshal.SizeOf(ChildNode);
            MinimumBoundingBox = new MinimumBoundingBox<CoordinateType>(byteData, offset, endAddress);
        }

        public override Byte[] GetBytes()
        {
            Int32 index = 0;
            Byte[] data = new Byte[GetSize()];
            ChildNode.GetBytes().CopyTo(data, index);
            index += Marshal.SizeOf(ChildNode);
            MinimumBoundingBox.GetBytes().CopyTo(data, index);
            return data;
        }
        public override Int32 GetSize()
        {
            return MinimumBoundingBox.GetSize() + Marshal.SizeOf(ChildNode.GetType());
        }

        public static Int32 GetSize(Int32 dimension)
        {
            return MinimumBoundingBox<CoordinateType>.GetSize(dimension) + Address.NullAddress.GetSize();
        }
    }
}
