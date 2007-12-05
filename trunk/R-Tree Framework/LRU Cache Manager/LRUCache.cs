using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;
using System.IO;

namespace Edu.Psu.Cse.R_Tree_Framework.CacheManagers
{
    public class LRUCacheManager : CacheManager
    {
        public event CacheEventHandler PageFault;
        public event CacheEventHandler PageWrite;

        protected Dictionary<Guid, Int64> addressTranslationTable;
        protected Dictionary<Int64, Page> pageTranslationTable;
        protected SortedList<Int64, Page> leastRecentlyUsed;
        protected Queue<Int64> freePages;
        protected List<Page> dirtyPages;
        protected String storageFileLocation;
        protected FileStream storageReader;
        protected Int32 pageSize, cacheSize;
        protected Int64 nextPageAddress;

        protected virtual Int64 NextPageAddress
        {
            get
            {
                Int64 address = nextPageAddress;
                nextPageAddress += PageSize;
                return address;
            }
            set { nextPageAddress = value; }
        }
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
        protected virtual SortedList<Int64, Page> LeastRecentlyUsed
        {
            get { return leastRecentlyUsed; }
            set { leastRecentlyUsed = value; }
        }
        protected virtual List<Page> DirtyPages
        {
            get { return dirtyPages; }
            set { dirtyPages = value; }
        }
        protected virtual Queue<Int64> FreePages
        {
            get { return freePages; }
            set { freePages = value; }
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
            LeastRecentlyUsed = new SortedList<Int64, Page>();
            DirtyPages = new List<Page>();
            FreePages = new Queue<Int64>();
        }
        
        public virtual Record LookupRecord(Guid address)
        {
            Record record = new Record(LookupPage(address).Data);
            CacheOverflowCheck();
            return record;
        }
        public virtual Node LookupNode(Guid address)
        {
            Node node = new Node(LookupPage(address).Data);
            CacheOverflowCheck();
            return node;
        }
        public virtual void WritePageData(PageData data)
        {
            Page page;
            if (!AddressTranslationTable.ContainsKey(data.Address))
                page = AllocateNewPage(data.Address);
            else
            {
                Int64 address = AddressTranslationTable[data.Address];
                if (PageTranslationTable.ContainsKey(address))
                    page = PageTranslationTable[address];
                else
                    page = LoadPageFromMemory(address);
            }
            page.Data = data.GeneratePageData();
            DirtyPages.Add(page);
            if (LeastRecentlyUsed.ContainsValue(page))
                LeastRecentlyUsed.RemoveAt(LeastRecentlyUsed.IndexOfValue(page));
            LeastRecentlyUsed.Add(DateTime.Now.Ticks, page);
            CacheOverflowCheck();
        }
        public virtual void DeletePageData(PageData data)
        {
            Int64 address = AddressTranslationTable[data.Address];
            AddressTranslationTable.Remove(data.Address);
            Page page = PageTranslationTable[address];
            PageTranslationTable.Remove(address);
            LeastRecentlyUsed.RemoveAt(LeastRecentlyUsed.IndexOfValue(page));
            while(DirtyPages.Contains(page))
                DirtyPages.Remove(page);
            FreePages.Enqueue(address);
        }
        protected virtual Page LookupPage(Guid address)
        {
            Page page;
            Int64 offset = AddressTranslationTable[address];
            if (PageTranslationTable.ContainsKey(offset))
            {
                page = PageTranslationTable[offset];
                LeastRecentlyUsed.RemoveAt(LeastRecentlyUsed.IndexOfValue(page));
            }
            else
            {
                page = LoadPageFromMemory(offset);
                PageTranslationTable.Add(page.Address, page);
            }
            LeastRecentlyUsed.Add(DateTime.Now.Ticks, page);
            return page;
        }
        protected virtual Page LoadPageFromMemory(Int64 offset)
        {
            Byte[] data = new Byte[PageSize];
            StorageReader.Seek(offset - offset % PageSize, SeekOrigin.Begin);
            StorageReader.Read(data, 0, PageSize);
            Page page = new Page(Guid.NewGuid(), offset, data);
            PageFault(this, new LRUCacheEventArgs(page));
            return page;
        }
        protected virtual void WritePageToMemory(Page page)
        {
            StorageReader.Seek(page.Address, SeekOrigin.Begin);
            StorageReader.Write(page.Data, 0, PageSize);
            PageWrite(this, new LRUCacheEventArgs(page));
        }
        protected virtual void EvictPage()
        {
            Page page = leastRecentlyUsed[leastRecentlyUsed.Keys[0]];
            if (DirtyPages.Contains(page))
                WritePageToMemory(page);
            LeastRecentlyUsed.RemoveAt(0);
            while (DirtyPages.Contains(page))
                DirtyPages.Remove(page);
            PageTranslationTable.Remove(page.Address);
        }
        protected virtual void CacheOverflowCheck()
        {
            while (PageTranslationTable.Count > CacheSize)
                EvictPage();
        }
        protected virtual Page AllocateNewPage(Guid address)
        {
            Int64 pageAddress;
            if (FreePages.Count > 0)
                pageAddress = FreePages.Dequeue();
            else
                pageAddress = NextPageAddress;
            AddressTranslationTable.Add(address, pageAddress);
            Page newPage = new Page(Guid.NewGuid(), pageAddress, new Byte[PageSize]);
            PageTranslationTable.Add(pageAddress, newPage);
            return newPage;
        }
        public virtual void FlushCache()
        {
            while (PageTranslationTable.Count > 0)
                EvictPage();
        }
    }
}