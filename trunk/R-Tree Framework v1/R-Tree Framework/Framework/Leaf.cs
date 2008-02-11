using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class Leaf : Node
    {
        public Leaf(Address parent)
            : base(parent, typeof(Record))
        {
        }
        public Leaf(Address address, Byte[] pageData)
            : base(address, typeof(Record), pageData)
        {
        }
        protected override void AddNodeEntry(Byte[] entryData)
        {
            NodeEntries.Add(new LeafEntry(entryData));
        }
        protected override Byte ChildTypeID
        {
            get { return (Byte)NodeChildType.Record; }
        }
        protected override Byte TypeID
        {
            get { return (Byte)PageDataType.Leaf; }
        }
    }
}
