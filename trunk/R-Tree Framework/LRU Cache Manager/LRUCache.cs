using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Psu.Cse.R_Tree_Framework.CacheManagers.LRU_Cache
{
    public class Page{}
    public class LRUCacheManager
    {
        protected Dictionary<Guid, Int64> addressTranslationTable;
        protected Dictionary<Int64, Page> pageIDTranslationTable;
        protected leastRecentlyUsedList;
        protected String storageFileLocation;
        protected FileStream storageReader;
        protected Int32 pageSize, cacheSize;

        protected Int32 PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        protected Int32 CacheSize
        {
            get { return cacheSize; }
            set { cacheSize = value; }
        }
        protected String StorageFileLocation
        {
            get { return storageFileLocation; }
            set { storageFileLocation = value; }
        }
        protected Dictionary<Guid, Int32> AddressTranslationTable
        {
            get { return addressTranslationTable; }
            set { addressTranslationTable = value; }
        }
        protected  FileStream StorageReader
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

        }

        public Record LookupRecord(Guid address)
        {
            Int64 offset = AddressTranslationTable[address];
            Byte[] recordData;
            StorageReader.Seek(offset, SeekOrigin.Begin);
            StorageReader.Read(recordData, 0, PageSize);
            //Reader byte sequence of a record at the offset location

            return new record(recordData);
        }
    }
}

/* todo
define page 
make abstract dictionary containing the pages for quick lookup
make abstract priority queue used to determine which page is evicted
function for writing back abstract page
function for creating abstract new page worth of data
function for generating abstract node from data indexer abstract page
function for saving abstract ndoe to abstract page worth of data
function for reading abstract page of data and creating abstract record
function for saving abstract record as abstract page of data
*/