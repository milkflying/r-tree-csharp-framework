using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class Node : PageData
    {
        protected List<NodeEntry> nodeEntries;
        protected Guid address, parent;
        protected Type childType;

        public Type ChildType
        {
            get { return childType; }
            protected set { childType = value; }
        }

        public Guid Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public Guid Address
        {
            get { return address; }
            protected set { address = value; }
        }

        public List<NodeEntry> NodeEntries
        {
            get { return nodeEntries; }
            protected set { nodeEntries = value; }
        }

        public Node(int maxNodeEntries, Guid parent, Type childType)
        {
            Address = Guid.NewGuid();
            Parent = parent;
            ChildType = childType;
            nodeEntries = new List<NodeEntry>(maxNodeEntries);
        }
        public Node(Guid address, Type childType, Byte[] pageData)
        {
            Address = address;
            ChildType = childType;
            Byte[] parentAddress = new Byte[16];
            Int32 index = 32, nodeEntryCount;

            Array.Copy(pageData, index, parentAddress, 0, 16);
            Parent = new Guid(parentAddress);
            index += 16;

            nodeEntryCount = BitConverter.ToInt32(pageData, index);
            index += 4;

            NodeEntries = new List<NodeEntry>(nodeEntryCount);
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
            Double minX = nodeEntries[0].MinimumBoundingBox.MinX,
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
            Int32 index = 0;
            Byte[] data = new Byte[Constants.NODE_SIZE];
            
            this.GetType().GUID.ToByteArray().CopyTo(data, index);
            index += 16;

            ChildType.GUID.ToByteArray().CopyTo(data, index);
            index += 16;

            Parent.ToByteArray().CopyTo(data, index);
            index += 16;

            BitConverter.GetBytes(NodeEntries.Count).CopyTo(data, index);
            index += 4;

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
