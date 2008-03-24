using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Query;

namespace R_Tree_Framework.Index
{
    public abstract class SpatialIndex : R_Tree_Framework.Index.Index
    {
        public abstract List<Int32> Search(SpatialQuery query);

        public SpatialIndex(Int32 dimension, Int32 pageSize)
            : base(dimension, pageSize)
        {
        }
    }
}
