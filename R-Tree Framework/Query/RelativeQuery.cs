using System;
using System.Collections.Generic;
using System.Text;

namespace R_Tree_Framework.Query
{
    public abstract class RelativeQuery<CoordinateType> : SpatialQuery<CoordinateType> where CoordinateType : struct, IComparable
    {
    }
}
