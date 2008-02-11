using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.Indexes
{
    public class NodeEntryLowerAxisComparerY : IComparer<NodeEntry>
    {
        public Int32 Compare(NodeEntry a, NodeEntry b)
        {
            return a.MinimumBoundingBox.MinY.CompareTo(b.MinimumBoundingBox.MinY);
        }
    }
}
