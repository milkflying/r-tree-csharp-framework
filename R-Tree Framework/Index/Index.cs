using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Utility;
using R_Tree_Framework.Cache_Manager;
using R_Tree_Framework.Query;

namespace R_Tree_Framework.Index
{
    public abstract class Index : IndexObject
    {
        protected Node rootNode;
        protected Int32 dimension, pageSize;
        private CacheManager cacheManager;

        protected CacheManager CacheManager
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
        protected Node RootNode
        {
            get { return rootNode; }
            set { rootNode = value; }
        }


        public Index(Int32 dimension, Int32 pageSize)
        {
            Dimension = dimension;
            PageSize = pageSize;
            RootNode = new LeafNode();
        }

        public abstract List<Int32> Search(Query.Query query);
    }
}
