using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class KNearestNeighborQuery : Query
    {
        protected Int32 k;
        protected Double x, y;

        public Int32 K
        {
            get { return k; }
            protected set { k = value; }
        }
        public Double X
        {
            get { return x; }
            protected set { x = value; }
        }
        public Double Y
        {
            get { return y; }
            protected set { y = value; }
        }

        public KNearestNeighborQuery(Int32 k, Double x, Double y)
        {
            this.k = k;
            this.x = x;
            this.y = y;
        }
    }
}
