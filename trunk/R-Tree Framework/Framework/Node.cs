using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class Node : PageData
    {
        protected List<NodeEntry> nodeEntries;
        protected Address address, parent;
        protected Type childType;

        public Type ChildType
        {
            get { return childType; }
            protected set { childType = value; }
        }

        protected virtual Byte ChildTypeID
        {
            get
            {
                if (ChildType.Equals(typeof(Leaf)))
                    return (Byte)NodeChildType.Leaf;
                else
                    return (Byte)NodeChildType.Node;
            }
        }
        protected virtual Byte TypeID
        {
            get { return (Byte)PageDataType.Node; }
        }

        public Address Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public Address Address
        {
            get { return address; }
            protected set { address = value; }
        }

        public List<NodeEntry> NodeEntries
        {
            get { return nodeEntries; }
            protected set { nodeEntries = value; }
        }

        public Node(Address parent, Type childType)
        {
            Address = Address.NewAddress();
            Parent = parent;
            ChildType = childType;
            nodeEntries = new List<NodeEntry>(Constants.MAXIMUM_ENTRIES_PER_NODE + 1);
        }
        public Node(Address address, Type childType, Byte[] pageData)
        {
            Address = address;
            ChildType = childType;
            Byte[] parentAddress = new Byte[Constants.ADDRESS_SIZE];
            Int32 index = 2, nodeEntryCount;

            Array.Copy(pageData, index, parentAddress, 0, Constants.ADDRESS_SIZE);
            Parent = new Address(parentAddress);
            index += Constants.ADDRESS_SIZE;

            nodeEntryCount = BitConverter.ToInt16(pageData, index);
            index += 2;

            NodeEntries = new List<NodeEntry>(Constants.MAXIMUM_ENTRIES_PER_NODE + 1);
            for (int i = 0; i < nodeEntryCount; i++)
            {
                Byte[] entryData = new Byte[Constants.NODE_ENTRY_SIZE];
                Array.Copy(pageData, index, entryData, 0, Constants.NODE_ENTRY_SIZE);
                AddNodeEntry(entryData);
                index += Constants.NODE_ENTRY_SIZE;
            }
        }

        protected virtual void AddNodeEntry(Byte[] entryData)
        {
            NodeEntries.Add(new NodeEntry(entryData));
        }
        public MinimumBoundingBox CalculateMinimumBoundingBox()
        {
            Single minX = nodeEntries[0].MinimumBoundingBox.MinX,
                minY = nodeEntries[0].MinimumBoundingBox.MinY,
                maxX = nodeEntries[0].MinimumBoundingBox.MaxX, 
                maxY = nodeEntries[0].MinimumBoundingBox.MaxY;
            foreach (NodeEntry node in nodeEntries)
            {
                if (node.MinimumBoundingBox.MinX < minX)
                    minX = node.MinimumBoundingBox.MinX;
                if (node.MinimumBoundingBox.MinY < minY)
                    minY = node.MinimumBoundingBox.MinY;
                if (node.MinimumBoundingBox.MaxX > maxX)
                    maxX = node.MinimumBoundingBox.MaxX;
                if (node.MinimumBoundingBox.MaxY > maxY)
                    maxY = node.MinimumBoundingBox.MaxY;
            }
            return new MinimumBoundingBox(minX, minY, maxX, maxY);
        }

        public Byte[] GeneratePageData()
        {
            Int32 index = 2;
            Byte[] data = new Byte[Constants.NODE_SIZE];

            data[0] = TypeID;
            data[1] = ChildTypeID;

            Parent.ToByteArray().CopyTo(data, index);
            index += Constants.ADDRESS_SIZE;

            BitConverter.GetBytes((Int16)NodeEntries.Count).CopyTo(data, index);
            index += 2;

            foreach (NodeEntry entry in NodeEntries)
            {
                entry.GetBytes().CopyTo(data, index);
                index += Constants.NODE_ENTRY_SIZE;
            }

            return data;
        }
        public void AddNodeEntry(NodeEntry child)
        {
            nodeEntries.Add(child);
        }
        public void RemoveNodeEntry(NodeEntry child)
        {
            nodeEntries.Remove(child);
        }
    }
}
