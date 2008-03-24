using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Query;

namespace R_Tree_Framework.Index
{
    public abstract class SpatialIndex<CoordinateType> : R_Tree_Framework.Index.Index<CoordinateType> where CoordinateType : struct, IComparable
    {
        public abstract List<Int32> Search(SpatialQuery<CoordinateType> query);

        public SpatialIndex(Int32 dimension, Int32 pageSize)
            : base(dimension, pageSize)
        {
        }
    }
}
