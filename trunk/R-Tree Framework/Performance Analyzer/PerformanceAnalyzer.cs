using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.Performance_Metrics
{
    public class PerformanceAnalyzer
    {
        protected Int32 pageFaults;
        protected CacheManager cache;

        public virtual Int32 PageFaults
        {
            get { return pageFaults; }
            protected set { pageFaults = value; }
        }
        public virtual CacheManager Cache
        {
            get { return cache; }
            protected set { cache = value; }
        }

        public PerformanceAnalyzer(CacheManager cache)
        {
            PageFaults = 0;
            Cache = cache;
            cache.PageFault += new CacheEventHandler(PageFaulted);
            cache.PageWrite += new CacheEventHandler(PageWritten);
        }

        protected virtual void PageWritten(object sender, EventArgs args)
        {
        }

        protected virtual void PageFaulted(object sender, EventArgs args)
        {
            PageFaults++;
        }
    }
}
