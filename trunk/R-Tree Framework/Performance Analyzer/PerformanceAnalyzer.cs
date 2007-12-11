using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;
using System.Diagnostics;

namespace Edu.Psu.Cse.R_Tree_Framework.Performance_Metrics
{
    public class PerformanceAnalyzer
    {
        protected DateTime operationStart, operationEnd;
        protected TimeSpan startCPUTime, endCPUTime;

        protected virtual TimeSpan StartCPUTime
        {
            get { return startCPUTime; }
            set { startCPUTime = value; }
        }
        protected virtual TimeSpan EndCPUTime
        {
            get { return endCPUTime; }
            set { endCPUTime = value; }
        }
        protected Int32 pageFaults, pageWrites;
        protected CacheManager cache;
        public virtual Int32 PageFaults
        {
            get { return pageFaults; }
            protected set { pageFaults = value; }
        }
        public virtual Int32 PageWrites
        {
            get { return pageWrites; }
            protected set { pageWrites = value; }
        }
        public virtual DateTime ExecutionTime
        {
            get { return OperationEnd - OperationStart; }
        }
        public virtual TimeSpan CPUTime
        {
            get { return EndCPUTime - StartCPUTime; }
        }
        public virtual CacheManager Cache
        {
            get { return cache; }
            protected set { cache = value; }
        }
        protected virtual DateTime OperationStart
        {
            get { return operationStart; }
            set { operationStart = value; }
        }
        protected virtual DateTime OperationEnd
        {
            get { return operationEnd; }
            set { operationEnd = value; }
        }
        public PerformanceAnalyzer(CacheManager cache)
        {
            PageFaults = 0;
            PageWrites = 0;
            OperationStart = DateTime.Now;
            OperationEnd = OperationStart;
            startCPUTime = Process.GetCurrentProcess().TotalProcessorTime;
            endCPUTime = startCPUTime;
            Cache = cache;
            Cache.PageFault += new CacheEventHandler(PageFaulted);
            Cache.PageWrite += new CacheEventHandler(PageWritten);
        }

        protected virtual void PageWritten(object sender, EventArgs args)
        {
            PageWrites++;
        }
        protected virtual void PageFaulted(object sender, EventArgs args)
        {
            PageFaults++;
        }
        protected virtual void OperationEnded(object sender, EventArgs args)
        {
            endCPUTime = Process.GetCurrentProcess().TotalProcessorTime;
            OperationEnd = DateTime.Now;
        }
        protected virtual void OperationBegan(object sender, EventArgs args)
        {
            OperationStart = DateTime.Now;
            startCPUTime = Process.GetCurrentProcess().TotalProcessorTime;
        }
        public virtual void ClearStatistics()
        {
            PageWrites = 0;
            PageFaults = 0;
        }
    }
}
