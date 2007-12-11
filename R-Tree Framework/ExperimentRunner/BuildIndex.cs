using System;
using System.Collections.Generic;
using System.Text;

namespace ExperimentRunner
{
    public class BuildIndex : Experiment
    {
        public virtual String DataSetLocation
        {
            get
            {
                return String.Format(@"{0}\DataSet\{1}.{2}.dataset",
                    ROOT,
                    DataSet.ToString(),
                    Cardinality.ToString());
            }
        }
        public BuildIndex(
            DataSet dataSet,
            Cardinality cardinality,
            IndexType indexType,
            ReservationBufferSize reservationBufferSize,
            CacheType cacheType,
            CacheSize cacheSize)
            : base(dataSet, cardinality, indexType, reservationBufferSize, cacheType, cacheSize)
        {
        }

    }
}
