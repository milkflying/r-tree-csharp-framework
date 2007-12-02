using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class RangeQuery : RegionQuery
    {
        private Double centerX, centerY, radius;

        public Double CenterX
        {
            get { return centerX; }
            private set { centerX = value; }
        }

        public Double CenterY
        {
            get { return centerY; }
            private set { centerY = value; }
        }

        public Double Radius
        {
            get { return radius; }
            private set { radius = value; }
        }

        public RangeQuery(Double centerX, Double centerY, Double radius)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            this.radius = radius;
        }
    }
}
