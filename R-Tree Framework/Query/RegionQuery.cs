using System;
using System.Collections.Generic;
using System.Text;

namespace R_Tree_Framework.Query
{
    public abstract class RegionQuery : SpatialQuery
    {
        public RegionQuery(List<Single> queryPoint)
            : base(queryPoint)
        {
        }
    }
}
