using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Query
{
    public class RangeQuery<CoordinateType> : RegionQuery<CoordinateType> where CoordinateType : struct, IComparable
    {
        protected CoordinateType range;

        public virtual CoordinateType Range
        {
            get { return range; }
            protected set { range = value; }
        }

        public RangeQuery(List<CoordinateType> queryPoint, CoordinateType range)
            : base (queryPoint)
        {
            Range = range;
            range = range + range;
        }
    }
}
