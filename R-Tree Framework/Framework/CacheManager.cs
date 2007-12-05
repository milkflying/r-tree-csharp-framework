using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public delegate void CacheEventHandler(object sender, EventArgs args);

    public interface CacheManager
    {
        event CacheEventHandler PageFault;
        event CacheEventHandler PageWrite;

        Int32 CacheSize { get; }
        Int32 PageSize { get; }
        String StorageFileLocation { get; }
        void WritePageData(PageData data);
        void DeletePageData(PageData data);
        Node LookupNode(Guid address);
        Record LookupRecord(Guid address);
        void FlushCache();
    }
}
