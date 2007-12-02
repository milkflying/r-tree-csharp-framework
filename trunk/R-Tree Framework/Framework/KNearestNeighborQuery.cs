using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public struct KNearestNeighborQuery : Query
    {
        private Int32 k;
        private Record dataRecord;

        public Record DataRecord
        {
            get { return dataRecord; }
            private set { dataRecord = value; }
        }

        public Int32 K
        {
            get { return k; }
            private set { k = value; }
        }

        public KNearestNeighborQuery(Int32 k, Record dataRecord)
        {
            this.k = k;
            this.dataRecord = dataRecord;
        }
    }
}
