using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.Indexes
{
    public class R_Sharp_Tree : R_Star_Tree
    {
        public R_Sharp_Tree(Int32 minimumNodeOccupancy, Int32 maximumNodeOccupancy, CacheManager cache)
            : base(minimumNodeOccupancy, maximumNodeOccupancy, cache)
        {
        }
        /*public R_Sharp_Tree(String savedFileLocation, CacheManager cache)
            : base(savedFileLocation, cache)
        {
        }*/
    }
}
