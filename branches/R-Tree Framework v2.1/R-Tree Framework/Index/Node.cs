using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Utility;
using System.Runtime.InteropServices;

namespace R_Tree_Framework.Index
{
    public enum NodeType : byte {InteriorNodeType = 0, LeafNodeType = 1}

    public abstract class Node<CoordinateType> : IndexObject, IAddressable, ISavable where CoordinateType : struct, IComparable
    {
        protected List<NodeEntry<CoordinateType>> nodeEntries;
        protected Address memoryAddress;

        public virtual List<NodeEntry<CoordinateType>> NodeEntries
        {
            get { return new List<NodeEntry<CoordinateType>>(nodeEntries); }
            protected set { nodeEntries = value; }
        }
        public virtual Address MemoryAddress
        {
            get { return memoryAddress; }
            protected set { memoryAddress = value; }
        }
        public virtual Int32 NodeCount
        {
            get { return NodeEntries.Count; }
        }

        public Node()
        {
            NodeEntries = new List<NodeEntry<CoordinateType>>();
            MemoryAddress = Address.NextAddress;
        }
        public abstract Byte[] GetBytes();

        protected virtual Byte[] GetBytes(NodeType nodeType)
        {
            Int32 index = 0;
            Byte[] data = new Byte[GetSize()];
            BitConverter.GetBytes((Byte)nodeType).CopyTo(data, index);
            index += Marshal.SizeOf(nodeType.GetType());
            MemoryAddress.GetBytes().CopyTo(data, index);
            index += MemoryAddress.GetSize();
            BitConverter.GetBytes(NodeCount).CopyTo(data, index);
            index += Marshal.SizeOf(NodeCount.GetType());
            foreach (NodeEntry<CoordinateType> nodeEntry in NodeEntries)
            {
                nodeEntry.GetBytes().CopyTo(data, index);
                index += nodeEntry.GetSize();
            }
            return data;
        }
        public virtual Int32 GetSize()
        {
            if (NodeEntries.Count > 0)
                return NodeEntries[0].GetSize() * NodeCount + Marshal.SizeOf(NodeCount.GetType()) + Marshal.SizeOf(typeof(NodeType));
            else
                return Marshal.SizeOf(NodeCount.GetType());
        }
    }
}