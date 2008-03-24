using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Framework;

namespace R_Tree_Framework.Query
{
    public class KNearestNeighborQuery<CoordinateType> : RelativeQuery<CoordinateType> where CoordinateType : struct, IComparable
    {
    }
}
