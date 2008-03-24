using System;
using System.Collections.Generic;
using System.Text;

namespace R_Tree_Framework.Query
{
    public abstract class RegionQuery<CoordinateType> : SpatialQuery<CoordinateType> where CoordinateType : struct, IComparable
    {
        public RegionQuery(List<CoordinateType> queryPoint)
            : base(queryPoint)
        {
        }
    }
}
