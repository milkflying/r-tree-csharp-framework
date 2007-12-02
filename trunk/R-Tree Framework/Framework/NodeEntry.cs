using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public class NodeEntry
    {
        private MinimumBoundingBox minimumBoundingBox;
        private Guid child;

        public MinimumBoundingBox MinimumBoundingBox
        {
            get { return minimumBoundingBox; }
            set { minimumBoundingBox = value; }
        }
        public Guid Child
        {
            get { return child; }
            private set { child = value; }
        }

        public NodeEntry(MinimumBoundingBox minimumBoundingBox, Guid child)
        {
            MinimumBoundingBox = minimumBoundingBox;
            Child = child;
        }
    }
}
