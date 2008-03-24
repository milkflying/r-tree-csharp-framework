using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Utility;
using R_Tree_Framework.Cache_Manager;
using R_Tree_Framework.Query;

namespace R_Tree_Framework.Index
{
    public abstract class Index<CoordinateType> : IndexObject where CoordinateType : struct, IComparable
    {
        protected Node<CoordinateType> rootNode;
        protected Int32 dimension, pageSize;
        private CacheManager<CoordinateType> cacheManager;

        protected CacheManager<CoordinateType> CacheManager
        {
            get { return cacheManager; }
            set { cacheManager = value; }
        }

        public virtual Int32 PageSize
        {
            get { return pageSize; }
            protected set { pageSize = value; }
        }
        public virtual Int32 Dimension
        {
            get { return dimension; }
            protected set { dimension = value; }
        }
        protected Node<CoordinateType> RootNode
        {
            get { return rootNode; }
            set { rootNode = value; }
        }


        public Index(Int32 dimension, Int32 pageSize)
        {
            Dimension = dimension;
            PageSize = pageSize;
            RootNode = new LeafNode<CoordinateType>();
        }

        public abstract List<Int32> Search(Query.Query query);
    }
}
