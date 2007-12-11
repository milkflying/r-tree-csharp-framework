using System;
using System.Collections.Generic;
using System.Text;

namespace ExperimentRunner
{
    public enum DataSet { Uniform, Real }
    public enum Cardinality { Small, Large }
    public enum IndexType { R_Star_Tree, Flash_R_Tree, Flash_R_Tree_Extended, R_Sharp_Tree }
    public enum ReservationBufferSize { None, Small, Medium, Large }

    public abstract class Experiment
    {
        protected static String
            ROOT = @"C:\Users\Mike\Documents\R-Tree Framework\trunk\Experiments";
        
        protected DataSet dataSet;
        protected Cardinality cardinality;
        protected IndexType indexType;
        protected ReservationBufferSize reservationBufferSize;

        public virtual DataSet DataSet
        {
            get { return dataSet; }
            protected set { dataSet = value; }
        }
        public virtual Cardinality Cardinality
        {
            get { return cardinality; }
            protected set { cardinality = value; }
        }
        public virtual IndexType IndexType
        {
            get { return indexType; }
            protected set { indexType = value; }
        }
        public virtual ReservationBufferSize ReservationBufferSize
        {
            get { return reservationBufferSize; }
            protected set { reservationBufferSize = value; }
        }

        public virtual String CacheSaveLocation
        {
            get
            {
                return String.Format(@"{0}\Cache\{1}.{2}.{3}.{4}.cache",
                    ROOT,
                    DataSet.ToString(),
                    Cardinality.ToString(),
                    IndexType.ToString(), 
                    ReservationBufferSize.ToString());
            }
        }
        public virtual String DatabaseSaveLocation
        {
            get
            {
                return String.Format(@"{0}\Database\{1}.{2}.{3}.{4}.database",
                    ROOT,
                    DataSet.ToString(),
                    Cardinality.ToString(),
                    IndexType.ToString(),
                    ReservationBufferSize.ToString());
            }
        }
        public virtual String IndexSaveLocation
        {
            get
            {
                return String.Format(@"{0}\Index\{1}.{2}.{3}.{4}.index",
                    ROOT,
                    DataSet.ToString(),
                    Cardinality.ToString(),
                    IndexType.ToString(),
                    ReservationBufferSize.ToString());
            }
        }
        public virtual String MemorySaveLocation
        {
            get
            {
                return String.Format(@"{0}\Memory\{1}.{2}.{3}.{4}.memory",
                    ROOT,
                    DataSet.ToString(),
                    Cardinality.ToString(),
                    IndexType.ToString(),
                    ReservationBufferSize.ToString());
            }
        }

        public Experiment(
            DataSet dataSet,
            Cardinality cardinality,
            IndexType indexType,
            ReservationBufferSize reservationBufferSize)
        {
            DataSet = dataSet;
            Cardinality = cardinality;
            IndexType = indexType;
            ReservationBufferSize = reservationBufferSize;
        }
    }
}
