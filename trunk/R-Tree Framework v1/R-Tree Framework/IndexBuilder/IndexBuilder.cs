using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Indexes;
using Edu.Psu.Cse.R_Tree_Framework.CacheManagers;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace IndexBuilder
{
    public class IndexBuilder
    {
        #region Constants

        protected static readonly Type R_TREE = typeof(R_Tree);
        protected static readonly Type R_STAR_TREE = typeof(R_Star_Tree);
        protected static readonly Type FLASH_R_TREE = typeof(Flash_R_Tree);
        protected static readonly Type FLASH_R_TREE_EXTENDED = typeof(Flash_R_Tree_Extended);
        protected static readonly Type R_SHARP_TREE = typeof(R_Sharp_Tree);

        #endregion
        #region Instance Variables

        protected String dataSetFileLocation,
            indexSaveFileLocation,
            cacheSaveFileLocation,
            memorySaveFileLocation;
        protected Type treeType;
        protected Index treeIndex;
        protected Int32 reservationBufferSize;
        protected CacheManager cache;

        #endregion
        #region Properties

        protected virtual String IndexSaveFileLocation
        {
            get { return indexSaveFileLocation; }
            set { indexSaveFileLocation = value; }
        }
        protected virtual String CacheSaveFileLocation
        {
            get { return cacheSaveFileLocation; }
            set { cacheSaveFileLocation = value; }
        }
        protected virtual String MemorySaveFileLocation
        {
            get { return memorySaveFileLocation; }
            set { memorySaveFileLocation = value; }
        }
        protected virtual String DataSetFileLocation
        {
            get { return dataSetFileLocation; }
            set { dataSetFileLocation = value; }
        }
        protected virtual Type TreeType
        {
            get { return treeType; }
            set { treeType = value; }
        }
        protected virtual Index TreeIndex
        {
            get { return treeIndex; }
            set { treeIndex = value; }
        }
        protected virtual Int32 ReservationBufferSize
        {
            get { return reservationBufferSize; }
            set { reservationBufferSize = value; }
        }
        protected virtual CacheManager Cache
        {
            get { return cache; }
            set { cache = value; }
        }

        #endregion
        #region Constructors

        public IndexBuilder(
            Type treeType,
            Int32 reservationBufferSize,
            CacheManager cache,
            String dataSetFileLocation,
            String indexSaveFileLocation,
            String cacheSaveFileLocation,
            String memorySaveFileLocation)
        {
            ReservationBufferSize = reservationBufferSize;
            TreeType = treeType;
            DataSetFileLocation = dataSetFileLocation;
            IndexSaveFileLocation = indexSaveFileLocation;
            CacheSaveFileLocation = cacheSaveFileLocation;
            MemorySaveFileLocation = memorySaveFileLocation;
            Cache = cache;
        }

        #endregion
        #region Public Methods

        public virtual void BuildIndex()
        {
            SelectTreeType();

            StreamReader reader = new StreamReader(DataSetFileLocation);
            while (!reader.EndOfStream)
            {
                String[] values = reader.ReadLine().Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Int32 recordId = Int32.Parse(values[0]);
                if (recordId % 2500 == 0)
                    Console.WriteLine(recordId.ToString());
                Single x = Single.Parse(values[1]), y = Single.Parse(values[2]);
                Record record = new Record(recordId, new MinimumBoundingBox(x, y, x, y));
                if (record.Address.ToString().Equals("18336"))
                {
                    Console.WriteLine("Here it is");
                    Console.ReadLine();
                }
                TreeIndex.Insert(record);
            }
            reader.Close();
            TreeIndex.SaveIndex(IndexSaveFileLocation, CacheSaveFileLocation, MemorySaveFileLocation);
        }

        #endregion
        #region Protected Methods

        protected virtual void SelectTreeType()
        {
            if (treeType == R_SHARP_TREE)
                throw new Exception("Tree type not yet implemented.");//TreeIndex = new R_Sharp_Tree();
            else if (treeType == FLASH_R_TREE_EXTENDED)
                TreeIndex = new Flash_R_Tree_Extended(Cache, ReservationBufferSize * Constants.INDEX_UNIT_ENTRIES_PER_SECTOR);
            else if (treeType == FLASH_R_TREE)
                TreeIndex = new Flash_R_Tree(Cache, ReservationBufferSize*Constants.INDEX_UNIT_ENTRIES_PER_SECTOR);
            else if (treeType == R_STAR_TREE)
                TreeIndex = new R_Star_Tree(Cache);
            else if (treeType == R_TREE)
                TreeIndex = new R_Tree(Cache);
            else throw new Exception("No such tree type.");
        }

        #endregion
    }
}
