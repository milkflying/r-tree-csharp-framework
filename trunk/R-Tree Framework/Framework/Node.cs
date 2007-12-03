using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class Node
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
        public Node(Byte[] pageData)
        {
        }
        public MinimumBoundingBox CalculateMinimumBoundingBox()
        {
            MinimumBoundingBox min = (nodeEntries[0] as NodeEntry).MinimumBoundingBox;
            foreach (NodeEntry node in nodeEntries)
                if (node.MinimumBoundingBox.GetArea() < min.GetArea())
                    min = node.MinimumBoundingBox;
            return min;
        }

        public Byte[] GeneratePageData()
        {
            return new Byte[1];
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
