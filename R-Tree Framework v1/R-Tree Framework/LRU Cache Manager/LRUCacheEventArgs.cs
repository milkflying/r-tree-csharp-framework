using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.CacheManagers
{
    public class LRUCacheEventArgs : EventArgs
    {
        protected Page page;

        public virtual Page Page
        {
            get { return page; }
            protected set { page = value; }
        }

        public LRUCacheEventArgs(Page page)
        {
        }
    }
}
