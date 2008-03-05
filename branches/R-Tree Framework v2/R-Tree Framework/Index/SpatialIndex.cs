using System;
using System.Collections.Generic;
using System.Text;

namespace R_Tree_Framework.Index
{
    public abstract class SpatialIndex<CoordinateType> : R_Tree_Framework.Index.Index<CoordinateType> where CoordinateType : struct, IComparable
    {
    }
}
