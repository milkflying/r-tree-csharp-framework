using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public struct MinimumBoundingBox
    {
        private Double maxX, maxY, minX, minY;

        public Double MinY
        {
            get { return minY; }
            set { minY = value; }
        }

        public Double MinX
        {
            get { return minX; }
            set { minX = value; }
        }

        public Double MaxY
        {
            get { return maxY; }
            set { maxY = value; }
        }

        public Double MaxX
        {
            get { return maxX; }
            set { maxX = value; }
        }

        public MinimumBoundingBox(Double minX, Double minY, Double maxX, Double maxY)
        {
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
        }

        public Double GetArea()
        {
            return (MaxX - MinX) * (MaxY - MinY);
        }

        public Double GetPerimeter()
        {
            return 2*((MaxX - MinX) + (MaxY - MinY));
        }
    }
}
