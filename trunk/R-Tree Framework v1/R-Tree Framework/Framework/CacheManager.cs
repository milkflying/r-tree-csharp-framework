using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public delegate void CacheEventHandler(object sender, EventArgs args);

    public interface CacheManager : IDisposable
    {
        event CacheEventHandler PageFault;
        event CacheEventHandler PageWrite;

        void WritePageData(PageData data);
        void DeletePageData(PageData data);
        Node LookupNode(Address address);
        Record LookupRecord(Address address);
        Sector LookupSector(Address address);
        void FlushCache();
        void SaveCache(String cacheSaveLocation, String memorySaveLocation);
    }
}
