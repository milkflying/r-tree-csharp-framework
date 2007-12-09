using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.Indexes
{
    public class NodeTranslationTable : CacheManager
    {
        #region Events

        public event CacheEventHandler PageFault;
        public event CacheEventHandler PageWrite;

        #endregion
        #region Instance Variables

        protected Int32 translationListSize;
        protected SortedList<Address, List<Address>> nodeToPagesTranslationTable;
        protected CacheManager memoryManager;
        protected List<IndexUnit> unitsToAdd;

        #endregion
        #region Properties

        protected virtual CacheManager MemoryManager
        {
            get { return memoryManager; }
            set { memoryManager = value; }
        }
        protected virtual SortedList<Address, List<Address>> NodeToPagesTranslationTable
        {
            get { return nodeToPagesTranslationTable; }
            set { nodeToPagesTranslationTable = value; }
        }
        protected virtual Int32 TranslationListSize
        {
            get { return translationListSize; }
            set { translationListSize = value; }
        }
        protected virtual List<IndexUnit> UnitsToAdd
        {
            get { return unitsToAdd; }
            set { unitsToAdd = value; }
        }

        #endregion
        #region Constructors

        public NodeTranslationTable(CacheManager underlyingCache)
        {
            MemoryManager = underlyingCache;
        }

        #endregion
        #region Public Methods

        public virtual Record LookupRecord(Address address)
        {
            return MemoryManager.LookupRecord(address);
        }
        public virtual Node LookupNode(Address address)
        {
            Node savedNode = MemoryManager.LookupNode(address);
            List<Address> nodeTranslationList = NodeToPagesTranslationTable[address];
            foreach (Address pageAddress in nodeTranslationList)
            {
                Sector sector = MemoryManager.LookupSector(pageAddress);
                foreach (IndexUnit nodeChange in sector.IndexUnits)
                    if (nodeChange.Node.Equals(address))
                        if (nodeChange.Operation == Operation.Insert)
                            savedNode.AddNodeEntry(new NodeEntry(nodeChange.ChildBoundingBox, nodeChange.Child));
                        else if (nodeChange.Operation == Operation.Delete)
                        {
                            NodeEntry entryToRemove = null;
                            foreach (NodeEntry entry in savedNode.NodeEntries)
                                if (entry.Child.Equals(nodeChange.Child))
                                    entryToRemove = entry;
                            if (entryToRemove == null)
                                throw new Exception();
                            savedNode.RemoveNodeEntry(entryToRemove);
                        }
                        else
                            foreach (NodeEntry entry in savedNode.NodeEntries)
                                if (entry.Child.Equals(nodeChange.Child))
                                    entry.MinimumBoundingBox = nodeChange.ChildBoundingBox;
            }
            return savedNode;
        }
        public virtual Sector LookupSector(Address address)
        {
            return MemoryManager.LookupSector(address);
        }
        public virtual void WritePageData(PageData data)
        {
            MemoryManager.WritePageData(data);
        }
        public virtual void DeletePageData(PageData data)
        {
            MemoryManager.DeletePageData(data);
        }
        public virtual void FlushCache()
        {
            MemoryManager.FlushCache();
        }
        public virtual void SaveCache(String cacheSaveLocation, String memorySaveLocation)
        {
            MemoryManager.SaveCache(cacheSaveLocation, memorySaveLocation);
        }
        public virtual void Add(IndexUnit indexUnit)
        {
            UnitsToAdd.Add(indexUnit);
        }
        public virtual void FlushIndexUnits()
        {
            List<Sector> newSectors = GenerateSectors(UnitsToAdd);
            foreach(Sector sector in newSectors)
            {
                foreach (IndexUnit indexUnit in sector.IndexUnits)
                    if (NodeToPagesTranslationTable.ContainsKey(indexUnit.Node))
                    {
                        NodeToPagesTranslationTable[indexUnit.Node].Add(sector.Address);
                        CheckSectorListOverflow(indexUnit.Node);
                    }
                    else
                    {
                        List<Address> pages = new List<Address>();
                        pages.Add(sector.Address);
                        NodeToPagesTranslationTable.Add(indexUnit.Node, pages);
                    }
                MemoryManager.WritePageData(sector);
            }
            UnitsToAdd.Clear();
        }
        public virtual void Dispose()
        {
            MemoryManager.Dispose();
        }

        #endregion
        #region Protected Methods

        protected virtual List<Sector> GenerateSectors(List<IndexUnit> indexUnits)
        {
            Dictionary<Address, List<IndexUnit>> groupings = new Dictionary<Address, List<IndexUnit>>();
            foreach (IndexUnit indexUnit in indexUnits)
            {
                if (!groupings.ContainsKey(indexUnit.Node))
                    groupings.Add(indexUnit.Node, new List<IndexUnit>());
                groupings[indexUnit.Node].Add(indexUnit);
            }
            List<Sector> sectors = new List<Sector>();
            foreach (KeyValuePair<Address, List<IndexUnit>> grouping in groupings)
            {
                Boolean inserted = false;
                foreach (Sector sector in sectors)
                {
                    if (sector.IndexUnits.Count + grouping.Value.Count <= Constants.INDEX_UNIT_ENTRIES_PER_SECTOR)
                    {
                        foreach (IndexUnit indexUnit in grouping.Value)
                            sector.AddIndexUnit(indexUnit);
                        inserted = true;
                        break;
                    }
                }
                if (!inserted)
                {
                    Sector sector = new Sector(Constants.INDEX_UNIT_ENTRIES_PER_SECTOR);
                    while (grouping.Value.Count > 0 && sector.IndexUnits.Count + 1 <= Constants.INDEX_UNIT_ENTRIES_PER_SECTOR)
                    {
                        sector.AddIndexUnit(grouping.Value[0]);
                        grouping.Value.RemoveAt(0);
                    }
                    sectors.Add(sector);
                }
            }
            return sectors;
        }
        protected virtual void CheckSectorListOverflow(Address nodeAddress)
        {
            List<Address> sectorList = NodeToPagesTranslationTable[nodeAddress];
            if (sectorList.Count > TranslationListSize)
            {
                MemoryManager.WritePageData(LookupNode(nodeAddress));
                NodeToPagesTranslationTable.Remove(nodeAddress);
            }
        }

        #endregion
    }
}
