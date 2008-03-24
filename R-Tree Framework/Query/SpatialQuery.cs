using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Utility;

namespace R_Tree_Framework.Query
{
    public abstract class SpatialQuery<CoordinateType> : Query where CoordinateType : struct, IComparable
    {
        protected List<CoordinateType> queryPoint;

        public virtual List<CoordinateType> QueryPoint
        {
            get { return queryPoint; }
            protected set { queryPoint = value; }
        }
        public virtual Int32 Dimension
        {
            get { return QueryPoint.Count; }
        }

        public SpatialQuery(List<CoordinateType> queryPoint)
        {
            QueryPoint = new List<CoordinateType>(queryPoint);
        }
    }
}
