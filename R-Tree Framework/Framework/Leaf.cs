using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class Leaf : Node
    {
        public Leaf(int maxRecordEntries, Guid parent)
            : base(maxRecordEntries, parent, typeof(Record))
        {
        }
        public Leaf(Byte[] pageData)
            : base(pageData)
        {
        }
        public void AddNodeEntry(LeafEntry record)
        {
            NodeEntries.Add(record);
        }
    }
}
