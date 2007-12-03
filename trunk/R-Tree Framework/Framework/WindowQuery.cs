using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class WindowQuery : RegionQuery
    {
        protected Double minX, minY, maxX, maxY;

        public Double MinX
        {
            get { return minX; }
            protected set { minX = value; }
        }

        public Double MinY
        {
            get { return minY; }
            protected set { minY = value; }
        }

        public Double MaxX
        {
            get { return maxX; }
            protected set { maxX = value; }
        }

        public Double MaxY
        {
            get { return maxY; }
            protected set { maxY = value; }
        }

        public WindowQuery(Double minX, Double minY, Double maxX, Double maxY)
        {
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
        }
    }
}
