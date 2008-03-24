using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Utility;

namespace R_Tree_Framework.Query
{
    public abstract class SpatialQuery : Query
    {
        protected List<Single> queryPoint;

        public virtual List<Single> QueryPoint
        {
            get { return queryPoint; }
            protected set { queryPoint = value; }
        }
        public virtual Int32 Dimension
        {
            get { return QueryPoint.Count; }
        }

        public SpatialQuery(List<Single> queryPoint)
            :base()
        {
            QueryPoint = new List<Single>(queryPoint);
        }
    }
}
