using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
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

        public NodeTranslationTable(CacheManager underlyingCache, Int32 translationListSize)
        {
            TranslationListSize = translationListSize;
            MemoryManager = underlyingCache;
            NodeToPagesTranslationTable = new SortedList<Address, List<Address>>();
            UnitsToAdd = new List<IndexUnit>();
        }
        public NodeTranslationTable(String tableSaveLocation, CacheManager underlyingCache)
        {
            MemoryManager = underlyingCache;
            NodeToPagesTranslationTable = new SortedList<Address, List<Address>>();
            UnitsToAdd = new List<IndexUnit>();

            StreamReader reader = new StreamReader(tableSaveLocation);

            TranslationListSize = Int32.Parse(reader.ReadLine());
            reader.ReadLine();
            String buffer;
            if (!(buffer = reader.ReadLine()).Equals("UnitsToAdd"))
                while (!buffer.Equals("UnitsToAdd"))
                {
                    Address nodeAddress = new Address(reader.ReadLine());
                    reader.ReadLine();
                    List<Address> sectorAddresses = new List<Address>();
                    while (!(buffer = reader.ReadLine()).Equals("UnitsToAdd") && !buffer.Equals("Node ID"))
                        sectorAddresses.Add(new Address(buffer));
                    NodeToPagesTranslationTable.Add(nodeAddress, sectorAddresses);
                }
            while (!reader.EndOfStream)
                UnitsToAdd.Add(new IndexUnit(Encoding.UTF8.GetBytes(reader.ReadLine())));
            reader.Close();

            MemoryManager.PageFault += new CacheEventHandler(PageFaulted);
            MemoryManager.PageWrite += new CacheEventHandler(PageWritten);
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
            if (NodeToPagesTranslationTable.ContainsKey(address))
            {
                List<Address> nodeTranslationList = NodeToPagesTranslationTable[address];
                foreach (Address sectorAddress in nodeTranslationList)
                {
                    Sector sector = MemoryManager.LookupSector(sectorAddress);
                    foreach (IndexUnit nodeChange in sector.IndexUnits)
                        if (nodeChange.Node.Equals(address))
                            if (nodeChange.Operation == Operation.Insert)
                                if (savedNode is Leaf)
                                    savedNode.AddNodeEntry(new LeafEntry(nodeChange.ChildBoundingBox, nodeChange.Child));
                                else
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
            }
            foreach (IndexUnit nodeChange in UnitsToAdd)
            {
                if (nodeChange.Node.Equals(address))
                    if (nodeChange.Operation == Operation.Insert)
                        if (savedNode is Leaf)
                            savedNode.AddNodeEntry(new LeafEntry(nodeChange.ChildBoundingBox, nodeChange.Child));
                        else
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
            if (data is Node)
            {
                NodeToPagesTranslationTable.Remove(data.Address);
                List<IndexUnit> unitsToRemove = new List<IndexUnit>();
                foreach (IndexUnit iu in UnitsToAdd)
                    if (iu.Node.Equals(data.Address))
                        unitsToRemove.Add(iu);
                foreach (IndexUnit iu in unitsToRemove)
                    UnitsToAdd.Remove(iu);
            }
        }
        public virtual void DeletePageData(PageData data)
        {
            MemoryManager.DeletePageData(data);
            if (data is Node)
            {
                NodeToPagesTranslationTable.Remove(data.Address);
                List<IndexUnit> unitsToRemove = new List<IndexUnit>();
                foreach (IndexUnit iu in UnitsToAdd)
                    if (iu.Node.Equals(data.Address))
                        unitsToRemove.Add(iu);
                foreach (IndexUnit iu in unitsToRemove)
                    UnitsToAdd.Remove(iu);
            }
        }
        public virtual void FlushCache()
        {
            MemoryManager.FlushCache();
        }
        public virtual void SaveCache(String cacheSaveLocation, String memorySaveLocation)
        {
            MemoryManager.SaveCache(cacheSaveLocation, memorySaveLocation);
        }
        public virtual void SaveTable(String tableSaveLocation)
        {
            StreamWriter writer = new StreamWriter(tableSaveLocation);

            writer.WriteLine(TranslationListSize);
            writer.WriteLine("NodeToPagesTranslationTable");
            foreach (KeyValuePair<Address, List<Address>> entry in NodeToPagesTranslationTable)
            {
                writer.WriteLine("Node ID");
                writer.WriteLine(entry.Key.ToString());
                writer.WriteLine("Page Addresses");
                foreach (Address pageAddress in entry.Value)
                    writer.WriteLine(pageAddress.ToString());
            }
            writer.WriteLine("UnitsToAdd");
            foreach (IndexUnit unit in UnitsToAdd)
            {
                writer.WriteLine(Encoding.UTF8.GetString(unit.GetBytes()));
            }
            writer.Close();
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
                MemoryManager.WritePageData(sector);
                foreach (IndexUnit iu in sector.IndexUnits)
                    UnitsToAdd.Remove(iu);
                foreach (IndexUnit indexUnit in sector.IndexUnits)
                    if (NodeToPagesTranslationTable.ContainsKey(indexUnit.Node))
                    {
                        if (!NodeToPagesTranslationTable[indexUnit.Node].Contains(sector.Address))
                        {
                            NodeToPagesTranslationTable[indexUnit.Node].Add(sector.Address);
                        }
                    }
                    else
                    {
                        List<Address> pages = new List<Address>();
                        pages.Add(sector.Address);
                        NodeToPagesTranslationTable.Add(indexUnit.Node, pages);
                    }
            }
            UnitsToAdd.Clear();
            CheckSectorListOverflow();
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
                while (grouping.Value.Count > 0)
                {
                    Boolean inserted = false;
                    foreach (Sector sector in sectors)
                    {
                        if (sector.IndexUnits.Count + grouping.Value.Count <= Constants.INDEX_UNIT_ENTRIES_PER_SECTOR)
                        {
                            foreach (IndexUnit indexUnit in grouping.Value)
                                sector.AddIndexUnit(indexUnit);
                            grouping.Value.Clear();
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
            }
            return sectors;
        }
        protected virtual void CheckSectorListOverflow()
        {
            List<Address> nodeAddresses = new List<Address>(NodeToPagesTranslationTable.Keys);
            foreach (Address nodeAddress in nodeAddresses)
            {
                List<Address> sectorList = NodeToPagesTranslationTable[nodeAddress];
                if (sectorList.Count > TranslationListSize)
                {
                    MemoryManager.WritePageData(LookupNode(nodeAddress));
                    NodeToPagesTranslationTable.Remove(nodeAddress);
                }
            }
        }
        protected virtual void PageWritten(object sender, EventArgs args)
        {
            if (PageWrite != null)
                PageWrite(this, args);
        }
        protected virtual void PageFaulted(object sender, EventArgs args)
        {
            if (PageFault != null)
                PageFault(this, args);
        }

        #endregion
    }
}
