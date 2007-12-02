using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class WindowQuery : RegionQuery
    {
        private Double minX, minY, maxX, maxY;

        public Double MinX
        {
            get { return minX; }
            private set { minX = value; }
        }

        public Double MinY
        {
            get { return minY; }
            private set { minY = value; }
        }

        public Double MaxX
        {
            get { return maxX; }
            private set { maxX = value; }
        }

        public Double MaxY
        {
            get { return maxY; }
            private set { maxY = value; }
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
