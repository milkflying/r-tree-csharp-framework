using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class LeafEntry : NodeEntry
    {
        public LeafEntry(MinimumBoundingBox minimumBoundingBox, Guid recordIdentifier)
            : base(minimumBoundingBox, recordIdentifier)
        {
        }
        public LeafEntry(Byte[] data)
            : base(data)
        {
        }
    }
}
