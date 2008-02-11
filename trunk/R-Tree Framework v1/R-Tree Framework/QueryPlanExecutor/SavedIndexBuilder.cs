using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Indexes;
using Edu.Psu.Cse.R_Tree_Framework.CacheManagers;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace QueryPlanExecutor
{
    public class SavedIndexBuilder
    {
        #region Constants

        protected static readonly Type R_TREE = typeof(R_Tree);
        protected static readonly Type R_STAR_TREE = typeof(R_Star_Tree);
        protected static readonly Type FLASH_R_TREE = typeof(Flash_R_Tree);
        protected static readonly Type FLASH_R_TREE_EXTENDED = typeof(Flash_R_Tree_Extended);
        protected static readonly Type R_SHARP_TREE = typeof(R_Sharp_Tree);

        #endregion
        #region Instance Variables

        protected String savedIndexFileLocation;
        protected Type treeType;
        protected Index treeIndex;
        protected CacheManager cache;

        #endregion
        #region Properties

        protected virtual String SavedIndexFileLocation
        {
            get { return savedIndexFileLocation; }
            set { savedIndexFileLocation = value; }
        }
        protected virtual Type TreeType
        {
            get { return treeType; }
            set { treeType = value; }
        }
        public  virtual Index TreeIndex
        {
            get { return treeIndex; }
            protected set { treeIndex = value; }
        }
        protected virtual CacheManager Cache
        {
            get { return cache; }
            set { cache = value; }
        }

        #endregion
        #region Constructors

        public SavedIndexBuilder(
            Type treeType, 
            CacheManager cache,
            String savedIndexFileLocation)
        {
            SavedIndexFileLocation = savedIndexFileLocation;
            TreeType = treeType;
            Cache = cache;
        }

        #endregion
        #region Public Methods

        public virtual void BuildIndex()
        {
            if (treeType == R_SHARP_TREE)
                throw new Exception("Tree type not yet implemented.");//TreeIndex = new R_Sharp_Tree();
            else if (treeType == FLASH_R_TREE_EXTENDED)
                TreeIndex = new Flash_R_Tree_Extended(SavedIndexFileLocation, Cache);
            else if (treeType == FLASH_R_TREE)
                TreeIndex = new Flash_R_Tree(SavedIndexFileLocation, Cache);
            else if (treeType == R_STAR_TREE)
                TreeIndex = new R_Star_Tree(SavedIndexFileLocation, Cache);
            else if (treeType == R_TREE)
                TreeIndex = new R_Tree(SavedIndexFileLocation, Cache);
            else throw new Exception("No such tree type.");
        }

        #endregion
    }
}
