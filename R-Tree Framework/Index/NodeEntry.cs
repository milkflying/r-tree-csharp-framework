using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Utility;

namespace R_Tree_Framework.Index
{
    public abstract class NodeEntry<CoordinateType> : IndexObject where CoordinateType : struct, IComparable
    {
        protected MinimumBoundingBox<CoordinateType> minimumBoundingBox;

        public virtual MinimumBoundingBox<CoordinateType> MinimumBoundingBox
        {
            get { return minimumBoundingBox; }
            protected set { minimumBoundingBox = value; }
        }

        

        public abstract Byte[] GetBytes();
        public abstract Int32 GetSize();
    }
}
