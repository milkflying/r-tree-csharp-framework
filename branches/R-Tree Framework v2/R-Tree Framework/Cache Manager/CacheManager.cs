using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Framework;
using R_Tree_Framework.Index;
using R_Tree_Framework.Utility;

namespace R_Tree_Framework.Cache_Manager
{
    public abstract class CacheManager<CoordinateType> : CacheManagerObject where CoordinateType : struct, IComparable
    {
        public abstract Node<CoordinateType> LookUpNode(Address memoryAddress);
    }
}
