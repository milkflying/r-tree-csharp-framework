using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public interface CacheManager
    {
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
