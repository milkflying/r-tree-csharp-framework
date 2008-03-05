using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Utility;

namespace R_Tree_Framework.Index
{
    public abstract class Index<CoordinateType> : IndexObject where CoordinateType : struct, IComparable
    {
        protected Node<CoordinateType> rootNode;
        protected Int32 maxEntriesPerInteriorNode, maxEntriesPerLeafNode, minEntriesPerInteriorNode, minEntriesPerLeafNode;
    }
}
