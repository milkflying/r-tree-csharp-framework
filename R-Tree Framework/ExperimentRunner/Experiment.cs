using System;
using System.Collections.Generic;
using System.Text;

namespace ExperimentRunner
{
    public enum DataSet { Uniform, Real }
    public enum Cardinality { Small, Large }
    public enum CacheSize { None, Medium, Large }
    public enum Drive { SDCard, FlashDrive, HardDrive }
    public enum IndexType { R_Star_Tree, Flash_R_Tree, Flash_R_Tree_Extended, R_Sharp_Tree }
    public enum CacheType { None, LRU, LevelProportional, HighestTreeLevel }
    public enum WorkPlan { Small, Medium, Large }
    public enum ReservationBufferSize { None, Small, Medium, Large }

    public class Experiment
    {
        private static String ROOT = @"C:\Users\Mike\Documents\R-Tree Framework\trunk\Experiments\",
            FLASH_DRIVE = "A:", SD_CARD = "B:", HARD_DRIVE = "C:";
        private static Int32 SECTOR_LIST_LENGTH = 4, MIN_FILL_FACTOR = 30;
        private DataSet dataSet;
        private Cardinality cardinality;
        private CacheSize cacheSize;
        private Drive drive;
        private IndexType indexType;
        private CacheType cacheType;
        private WorkPlan workPlan;
        private ReservationBufferSize reservationBufferSize;

        public Int32 MinimumFillFactor
        {
            get { return Experiment.MIN_FILL_FACTOR; }
        }
        public Int32 SectorListLength
        {
            get { return Experiment.SECTOR_LIST_LENGTH; }
        }
        public DataSet DataSet
        {
            get { return dataSet; }
            set { dataSet = value; }
        }
        public Cardinality Cardinality
        {
            get { return cardinality; }
            set { cardinality = value; }
        }
        public CacheSize CacheSize
        {
            get { return cacheSize; }
            set { cacheSize = value; }
        }
        public Drive Drive
        {
            get { return drive; }
            set { drive = value; }
        }
        public IndexType IndexType
        {
            get { return indexType; }
            set { indexType = value; }
        }
        public CacheType CacheType
        {
            get { return CacheType; }
            set { CacheType = value; }
        }
        public WorkPlan WorkPlan
        {
            get { return workPlan; }
            set { workPlan = value; }
        }
        public ReservationBufferSize ReservationBufferSize
        {
            get { return reservationBufferSize; }
            set { reservationBufferSize = value; }
        }

        public String CacheSaveLocation
        {
            get
            {
                return String.Format(@"{0}\Cache\{1}.{2}.{3}.{4}.{5}.{6}.{7}.{8}.{9}.cache",
              ROOT, DataSet.ToString(), Cardinality.ToString(), CacheSize.ToString(),
              Drive.ToString(), IndexType.ToString(), ReservationBufferSize.ToString(),
              CacheType.ToString(), WorkPlan.ToString());
            }
        }
        public String DatabaseSaveLocation
        {
            get
            {
                return String.Format(@"{0}\Database\{1}.{2}.{3}.{4}.{5}.{6}.{7}.{8}.{9}.database",
              ROOT, DataSet.ToString(), Cardinality.ToString(), CacheSize.ToString(),
              Drive.ToString(), IndexType.ToString(), ReservationBufferSize.ToString(),
              CacheType.ToString(), WorkPlan.ToString());
            }
        }
        public String DatabaseRunLocation
        {
            get
            {
                return String.Format(@"{0}\Database\{1}.{2}.{3}.{4}.{5}.{6}.{7}.{8}.{9}.database",
              GetDriveLetter(), DataSet.ToString(), Cardinality.ToString(), CacheSize.ToString(),
              Drive.ToString(), IndexType.ToString(), ReservationBufferSize.ToString(),
              CacheType.ToString(), WorkPlan.ToString());
            }
        }
        public String DataSetLocation
        {
            get
            {
                return String.Format(@"{0}\DataSet\{1}.{2}.dataset",
              ROOT, DataSet.ToString(), Cardinality.ToString());
            }
        }
        public String IndexSaveLocation
        {
            get
            {
                return String.Format(@"{0}\Index\{1}.{2}.{3}.{4}.{5}.{6}.{7}.{8}.{9}.index",
              ROOT, DataSet.ToString(), Cardinality.ToString(), CacheSize.ToString(),
              Drive.ToString(), IndexType.ToString(), ReservationBufferSize.ToString(),
              CacheType.ToString(), WorkPlan.ToString());
            }
        }
        public String MemorySaveLocation
        {
            get
            {
                return String.Format(@"{0}\Memory\{1}.{2}.{3}.{4}.{5}.{6}.{7}.{8}.{9}.memory",
              ROOT, DataSet.ToString(), Cardinality.ToString(), CacheSize.ToString(),
              Drive.ToString(), IndexType.ToString(), ReservationBufferSize.ToString(),
              CacheType.ToString(), WorkPlan.ToString());
            }
        }
        public String QueryPlanLocation
        {
            get
            {
                return String.Format(@"{0}\WorkPlan\{1}.workplan",
              ROOT, WorkPlan.ToString());
            }
        }
        public String ResultsSaveLocation
        {
            get
            {
                return String.Format(@"{0}\Results\{1}.{2}.{3}.{4}.{5}.{6}.{7}.{8}.{9}.results",
              ROOT, DataSet.ToString(), Cardinality.ToString(), CacheSize.ToString(),
              Drive.ToString(), IndexType.ToString(), ReservationBufferSize.ToString(),
              CacheType.ToString(), WorkPlan.ToString());
            }
        }

        public Experiment(DataSet dataSet, Cardinality cardinality,
            CacheSize cacheSize, Drive drive, IndexType indexType,
            CacheType cacheType, WorkPlan workPlan, ReservationBufferSize reservationBufferSize)
        {
            DataSet = dataSet;
            Cardinality = cardinality;
            CacheSize = cacheSize;
            Drive = drive;
            IndexType = indexType;
            CacheType = cacheType;
            WorkPlan = workPlan;
            ReservationBufferSize = reservationBufferSize;
        }

        private String GetDriveLetter()
        {
            if (Drive == Drive.FlashDrive)
                return FLASH_DRIVE;
            else if (Drive == Drive.HardDrive)
                return HARD_DRIVE;
            else if (Drive == Drive.SDCard)
                return SD_CARD;
            else
                throw new Exception("WTF");
        }
    }
}
