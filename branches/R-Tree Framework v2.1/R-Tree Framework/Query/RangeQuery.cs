using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Query
{
    public class RangeQuery : RegionQuery
    {
        protected Single range;

        public virtual Single Range
        {
            get { return range; }
            protected set { range = value; }
        }

        public RangeQuery(List<Single> queryPoint, Single range)
            : base (queryPoint)
        {
            Range = range;
        }
    }
}
