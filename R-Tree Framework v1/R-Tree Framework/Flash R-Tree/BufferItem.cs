using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.Indexes
{
    public struct BufferItem
    {
        private NodeEntry entry;

        public NodeEntry Entry
        {
            get { return entry; }
            set { entry = value; }
        }
        private Operation operation;

        public Operation Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        public BufferItem(NodeEntry entry, Operation operation)
        {
            this.entry = entry;
            this.operation = operation;
        }
    }
}
