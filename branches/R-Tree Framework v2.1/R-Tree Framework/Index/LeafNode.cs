using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Framework;
using R_Tree_Framework.Utility;
using R_Tree_Framework.Index.Exceptions;
using System.Runtime.InteropServices;

namespace R_Tree_Framework.Index
{
    public class LeafNode : Node
    {
        public virtual void AddNodeEntry(LeafNodeEntry nodeEntry)
        {
            if (nodeEntries.Contains(nodeEntry))
                throw new NodeEntryAlreadyAddedException();
            nodeEntries.Add(nodeEntry);
        }
        public virtual void RemoveNodeEntry(LeafNodeEntry nodeEntry)
        {
            if (!nodeEntries.Contains(nodeEntry))
                throw new NodeEntryAlreadyRemovedException();
            nodeEntries.Remove(nodeEntry);
        }

        public LeafNode()
            : base()
        {
        }
        public LeafNode(Byte[] byteData, Int32 dimension) : base()
        {
            Reconstruct(byteData, 0, byteData.Length, dimension);
        }
        public LeafNode(Byte[] byteData, Int32 offset, Int32 dimension)
        {
            Reconstruct(byteData, offset, byteData.Length, dimension);
        }
        public LeafNode(Byte[] byteData, Int32 offset, Int32 endAddress, Int32 dimension)
        {
            Reconstruct(byteData, offset, endAddress, dimension);
        }

        protected virtual void Reconstruct(Byte[] byteData, Int32 offset, Int32 endAddress, Int32 dimension)
        {
            MemoryAddress = Address.ReconstructAddress(byteData, offset);
            offset += MemoryAddress.GetSize();
            Int32 count = BitConverter.ToInt32(byteData, offset), nodeEntrySize = LeafNodeEntry.GetSize(dimension);
            offset += Marshal.SizeOf(count);
            for (int i = 0, e = offset + nodeEntrySize; i < count; i++, offset += nodeEntrySize, e += nodeEntrySize)
                AddNodeEntry(new LeafNodeEntry(byteData, offset, e));
        }

        public override byte[] GetBytes()
        {
            return GetBytes(NodeType.LeafNodeType);
        }
    }
}
