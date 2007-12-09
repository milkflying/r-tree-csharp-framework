using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class NodeEntry
    {
        protected MinimumBoundingBox minimumBoundingBox;
        protected Address child;

        public MinimumBoundingBox MinimumBoundingBox
        {
            get { return minimumBoundingBox; }
            set { minimumBoundingBox = value; }
        }
        public Address Child
        {
            get { return child; }
            protected set { child = value; }
        }

        public NodeEntry(MinimumBoundingBox minimumBoundingBox, Address child)
        {
            MinimumBoundingBox = minimumBoundingBox;
            Child = child;
        }

        public NodeEntry(Byte[] data)
        {
            Byte[] childAddress = new Byte[Constants.ADDRESS_SIZE];
            Array.Copy(data, childAddress, Constants.ADDRESS_SIZE);
            Child = new Address(childAddress);
            MinimumBoundingBox = new MinimumBoundingBox(
                BitConverter.ToSingle(data, Constants.ADDRESS_SIZE),
                BitConverter.ToSingle(data, Constants.ADDRESS_SIZE + 4),
                BitConverter.ToSingle(data, Constants.ADDRESS_SIZE + 8),
                BitConverter.ToSingle(data, Constants.ADDRESS_SIZE + 12));
        }

        public Byte[] GetBytes()
        {
            Int32 index = 0;
            Byte[] data = new Byte[Constants.NODE_ENTRY_SIZE];
            Child.ToByteArray().CopyTo(data, index);
            index += Constants.ADDRESS_SIZE;
            BitConverter.GetBytes(MinimumBoundingBox.MinX).CopyTo(data, index);
            index += 4;
            BitConverter.GetBytes(MinimumBoundingBox.MinY).CopyTo(data, index);
            index += 4;
            BitConverter.GetBytes(MinimumBoundingBox.MaxX).CopyTo(data, index);
            index += 4;
            BitConverter.GetBytes(MinimumBoundingBox.MaxY).CopyTo(data, index);
            return data;
        }
    }
}
