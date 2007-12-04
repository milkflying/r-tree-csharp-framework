using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;
using System.IO;

namespace Edu.Psu.Cse.R_Tree_Framework.CacheManagers.LRU_Cache
{
    public class LRUCacheManager
    {
        protected Dictionary<Guid, Int64> addressTranslationTable;
        protected Dictionary<Int64, Page> pageTranslationTable;
        protected SortedList<Int64, Page> leastRecentlyUsed;
 
        protected String storageFileLocation;
        protected FileStream storageReader;
        protected Int32 pageSize, cacheSize;

        public virtual Int32 PageSize
        {
            get { return pageSize; }
            protected set { pageSize = value; }
        }
        public virtual Int32 CacheSize
        {
            get { return cacheSize; }
            protected set { cacheSize = value; }
        }
        public virtual String StorageFileLocation
        {
            get { return storageFileLocation; }
            protected set { storageFileLocation = value; }
        }
        protected virtual Dictionary<Guid, Int64> AddressTranslationTable
        {
            get { return addressTranslationTable; }
            set { addressTranslationTable = value; }
        }
        protected virtual Dictionary<Int64, Page> PageTranslationTable
        {
            get { return pageTranslationTable; }
            set { pageTranslationTable = value; }
        }
        protected virtual SortedList<Int64, Page> LeastRecentlyUsedList
        {
            get { return leastRecentlyUsed; }
            set { leastRecentlyUsed = value; }
        }
        protected virtual Dictionary<Int64, Record> LoadedRecords
        {
            get { return loadedRecords; }
            set { loadedRecords = value; }
        }
        protected virtual Dictionary<Int64, Node> LoadedNodes
        {
            get { return loadedNodes; }
            set { loadedNodes = value; }
        }
        protected virtual FileStream StorageReader
        {
            get { return storageReader; }
            set { storageReader = value; }
        }

        public LRUCacheManager(String storageFileLocation, Int32 pageSize, Int32 numberOfPagesToCache)
        {
            StorageFileLocation = storageFileLocation;
            StorageReader = new FileStream(
                StorageFileLocation,
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.None,
                8,
                FileOptions.WriteThrough | FileOptions.RandomAccess);
            pageSize = pageSize;
            CacheSize = numberOfPagesToCache;
            AddressTranslationTable = new Dictionary<Guid, Int64>();
            PageTranslationTable = new Dictionary<Int64, Page>();
            LeastRecentlyUsedList = new SortedList<Int64, Page>();
        }

        public virtual Record LookupRecord(Guid address)
        {
            Int64 offset = AddressTranslationTable[address];
            if (PageTranslationTable.ContainsKey(offset))
            {
                Page page = PageTranslationTable[offset];
                LeastRecentlyUsedList.RemoveAt(LeastRecentlyUsedList.IndexOfValue(page));
                LeastRecentlyUsedList.Add(DateTime.Now.Ticks, page);
                return LoadedRecords[offset];
            }
            else
            {
                Page page = LoadPageFromMemory(offset);
                PageTranslationTable.Add(readPage.Address, readPage);
                return new record(recordData);
            }
        }
        public virtual Record LookupNode(Guid address)
        {
            Int64 offset = AddressTranslationTable[address];
            Byte[] recordData;
            StorageReader.Seek(offset - offset % PageSize, SeekOrigin.Begin);
            StorageReader.Read(recordData, 0, PageSize);
            return new record(recordData);
        }
        public virtual void WriteRecord(Record record)
        {
            Int64 address = AddressTranslationTable[record.Address];
            Page data;
            if (PageTranslationTable.ContainsKey(address))
                data = PageTranslationTable[address];
            else
                data = LoadPageFromMemory(address);
            data.Data = record.GeneratePageData();
        }
        public virtual void WriteNode(Node node)
        {
        }
        protected virtual Page LoadPageFromMemory(Int64 offset)
        {
            Byte[] data = new Byte[PageSize];
            StorageReader.Seek(offset, SeekOrigin.Begin);
            StorageReader.Read(recordData, 0, PageSize);
            Page readPage = new Page(Guid.NewGuid(), offset, data);
            if (leastRecentlyUsed.Count == CacheSize)
                EvictPage();
            LeastRecentlyUsedList.Add(DateTime.Now.Ticks, readPage);
        }
        protected virtual void EvictPage()
        {
            leastRecentlyUsed.RemoveAt(0);
        }

    }
}

/* todo
make abstract dictionary containing the pages for quick lookup
make abstract priority queue used to determine which page is evicted
function for writing back abstract page
function for creating abstract new page worth of data
function for generating abstract node from data indexer abstract page
function for saving abstract ndoe to abstract page worth of data
function for reading abstract page of data and creating abstract record
function for saving abstract record as abstract page of data
*/