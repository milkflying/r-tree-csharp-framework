using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Indexes;
using Edu.Psu.Cse.R_Tree_Framework.CacheManagers;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace IndexBuilder
{
    public class CacheBuilder
    {
        #region Constants

        protected static readonly Type LRU_CACHE = typeof(LRUCacheManager);
        protected static readonly Type LEVEL_PROPORTIONAL_CACHE = typeof(LevelProportionalCacheManager);
        protected static readonly Type HIGHEST_TREE_LEVEL_CACHE = typeof(HighestTreeLevelCacheManager);

        #endregion
        #region Instance Variables

        protected CacheManager cache;
        protected Type cacheType;
        protected String databaseFileLocation;
        protected Int32 pageSize, cacheSize;

        #endregion
        #region Properties

        public virtual CacheManager Cache
        {
            get { return cache; }
            protected set { cache = value; }
        }
        protected virtual Type CacheType
        {
            get { return cacheType; }
            set { cacheType = value; }
        }
        protected virtual String DatabaseFileLocation
        {
            get { return databaseFileLocation; }
            set { databaseFileLocation = value; }
        }
        protected virtual Int32 PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        protected virtual Int32 CacheSize
        {
            get { return cacheSize; }
            set { cacheSize = value; }
        }

        #endregion
        #region Constructors

        public CacheBuilder(Type cacheType, String databaseFileLocation, Int32 pageSize, Int32 cacheSize)
        {
            CacheType = cacheType;
            DatabaseFileLocation = databaseFileLocation;
            PageSize = pageSize;
            CacheSize = cacheSize;
        }

        #endregion
        #region Public Methods

        public virtual void BuildCache()
        {
            if (CacheType == HIGHEST_TREE_LEVEL_CACHE)
                throw new Exception("Cache type not yet implemented"); //Cache = new HighestTreeLevelCacheManager();
            else if (CacheType == LEVEL_PROPORTIONAL_CACHE)
                throw new Exception("Cache type not yet implemented"); //Cache = new LevelProportionalCacheManager();
            else if (CacheType == LRU_CACHE)
                Cache = new LRUCacheManager(DatabaseFileLocation, PageSize, CacheSize);
            else
                throw new Exception("No such cache type exists.");
        }

        #endregion
    }
}
