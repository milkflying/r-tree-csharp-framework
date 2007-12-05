using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class NodeEntry
    {
        protected MinimumBoundingBox minimumBoundingBox;
        protected Guid child;

        public MinimumBoundingBox MinimumBoundingBox
        {
            get { return minimumBoundingBox; }
            set { minimumBoundingBox = value; }
        }
        public Guid Child
        {
            get { return child; }
            protected set { child = value; }
        }

        public NodeEntry(MinimumBoundingBox minimumBoundingBox, Guid child)
        {
            MinimumBoundingBox = minimumBoundingBox;
            Child = child;
        }

        public NodeEntry(Byte[] data)
        {
            Byte[] childAddress = new Byte[16];
            Array.Copy(data, childAddress, 16);
            Child = new Guid(childAddress);
            MinimumBoundingBox = new MinimumBoundingBox(
                BitConverter.ToDouble(data, 16),
                BitConverter.ToDouble(data, 24),
                BitConverter.ToDouble(data, 32),
                BitConverter.ToDouble(data, 40));
        }

        public Byte[] GetBytes()
        {
            Byte[] data = new Byte[Constants.NODE_ENTRY_SIZE];
            Child.ToByteArray().CopyTo(data, 0);
            BitConverter.GetBytes(MinimumBoundingBox.MinX).CopyTo(data, 16);
            BitConverter.GetBytes(MinimumBoundingBox.MinY).CopyTo(data, 24);
            BitConverter.GetBytes(MinimumBoundingBox.MaxX).CopyTo(data, 32);
            BitConverter.GetBytes(MinimumBoundingBox.MaxY).CopyTo(data, 40);
            return data;
        }
    }
}
