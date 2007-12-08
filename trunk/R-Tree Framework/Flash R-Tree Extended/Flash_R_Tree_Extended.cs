using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.Indexes
{
    public class Flash_R_Tree_Extended : Flash_R_Tree
    {
        public Flash_R_Tree_Extended(Int32 minimumNodeOccupancy, Int32 maximumNodeOccupancy, CacheManager cache)
            : base(minimumNodeOccupancy, maximumNodeOccupancy, cache)
        {
        }
        public Flash_R_Tree_Extended(String savedFileLocation, CacheManager cache)
            : base(savedFileLocation, cache)
        {
        }
    }
}
