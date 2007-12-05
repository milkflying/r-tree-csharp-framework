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
        public Leaf(Guid address, Byte[] pageData)
            : base(address, typeof(Record), pageData)
        {
        }
        protected override void AddNodeEntry(Byte[] entryData)
        {
            NodeEntries.Add(new LeafEntry(entryData));
        }
    }
}
