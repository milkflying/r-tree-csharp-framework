using System;
using System.Collections.Generic;
using System.Text;

namespace ExperimentRunner
{
    public enum Drive { SDCard, FlashDrive, HardDrive }
    public enum WorkPlan { Small, Medium, Large }

    public class RunWorkPlan : Experiment
    {
        protected static String 
            FLASH_DRIVE = "A:", 
            SD_CARD = "B:", 
            HARD_DRIVE = "C:";
        
        protected Drive drive;
        protected WorkPlan workPlan;
        
        public virtual Drive Drive
        {
            get { return drive; }
            protected set { drive = value; }
        }
        public virtual WorkPlan WorkPlan
        {
            get { return workPlan; }
            protected set { workPlan = value; }
        }
        public virtual String DatabaseRunLocation
        {
            get
            {
                return String.Format(@"{0}\{1}.{2}.{3}.{4}.{5}.{6}.{7}.{8}.database",
               GetDriveLetter(),
               DataSet.ToString(),
               Cardinality.ToString(),
               CacheSize.ToString(),
               Drive.ToString(),
               IndexType.ToString(),
               ReservationBufferSize.ToString(),
               CacheType.ToString(),
               WorkPlan.ToString());
            }
        }
        public virtual String WorkPlanLocation
        {
            get
            {
                return String.Format(@"{0}\WorkPlan\{1}.workplan",
                    ROOT,
                    WorkPlan.ToString());
            }
        }
        public virtual String ResultsSaveLocation
        {
            get
            {
                return String.Format(@"{0}\Results\{1}.{2}.{3}.{4}.{5}.{6}.{7}.{8}.results",
               ROOT,
               DataSet.ToString(),
               Cardinality.ToString(),
               CacheSize.ToString(),
               Drive.ToString(),
               IndexType.ToString(),
               ReservationBufferSize.ToString(),
               CacheType.ToString(),
               WorkPlan.ToString());
            }
        }

        public RunWorkPlan(
            DataSet dataSet,
            Cardinality cardinality,
            IndexType indexType,
            ReservationBufferSize reservationBufferSize,
            CacheSize cacheSize,
            Drive drive,
            CacheType cacheType,
            WorkPlan workPlan)
            : base(dataSet, cardinality, indexType, reservationBufferSize, cacheType, cacheSize)
        {
            Drive = drive;
            WorkPlan = workPlan;
        }

        protected virtual String GetDriveLetter()
        {
            if (Drive == Drive.SDCard)
                return SD_CARD;
            else if (Drive == Drive.HardDrive)
                return HARD_DRIVE;
            else if (Drive == Drive.FlashDrive)
                return FLASH_DRIVE;
            else
                throw new Exception("Uh oh, not a drive type.");
        }

    }
}
