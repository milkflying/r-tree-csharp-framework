using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Indexes;
using Edu.Psu.Cse.R_Tree_Framework.CacheManagers;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace QueryPlanExecutor
{
    public class SavedCacheBuilder
    {
        #region Constants

        protected static readonly Type LRU_CACHE = typeof(LRUCacheManager);
        protected static readonly Type LEVEL_PROPORTIONAL_CACHE = typeof(LevelProportionalCacheManager);
        protected static readonly Type HIGHEST_TREE_LEVEL_CACHE = typeof(HighestTreeLevelCacheManager);

        #endregion
        #region Instance Variables

        protected CacheManager cache;
        protected Type cacheType;
        protected String savedCacheFileLocation, memoryRunLocation;

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
        protected virtual String SavedCacheFileLocation
        {
            get { return savedCacheFileLocation; }
            set { savedCacheFileLocation = value; }
        }
        protected virtual String MemoryRunLocation
        {
            get { return memoryRunLocation; }
            set { memoryRunLocation = value; }
        }

        #endregion
        #region Constructors

        public SavedCacheBuilder(Type cacheType, String savedCacheFileLocation, String memoryRunLocation)
        {
            CacheType = cacheType;
            SavedCacheFileLocation = savedCacheFileLocation;
            MemoryRunLocation = memoryRunLocation;
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
                Cache = new LRUCacheManager(SavedCacheFileLocation, MemoryRunLocation);
            else
                throw new Exception("No such cache type exists.");
        }

        #endregion
    }
}
