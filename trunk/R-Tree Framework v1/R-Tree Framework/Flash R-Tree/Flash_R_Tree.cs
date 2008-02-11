using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.Indexes
{
    public class Flash_R_Tree : R_Tree
    {
        #region Instance Variables

        protected NodeTranslationTable nodeTranslationTable;
        protected ReservationBuffer buffer;
        protected Int32 bufferSize;

        #endregion
        #region Properties

        protected virtual Int32 BufferSize
        {
            get { return bufferSize; }
            set { bufferSize = value; }
        }
        protected virtual ReservationBuffer Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }
        protected virtual NodeTranslationTable NodeTranslationTable
        {
            get { return nodeTranslationTable; }
            set { nodeTranslationTable = value; }
        }

        #endregion
        #region Constructors

        public Flash_R_Tree(CacheManager cache, Int32 reservationBufferSize)
            : base(cache)
        {
            BufferSize = reservationBufferSize;
            Buffer = new ReservationBuffer(BufferSize);
            NodeTranslationTable = new NodeTranslationTable(cache);
            Cache = NodeTranslationTable;
        }
        public Flash_R_Tree(String savedFileLocation, CacheManager cache)
            : base(savedFileLocation, cache)
        {
            StreamReader reader = new StreamReader(savedFileLocation);
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();
            BufferSize = Int32.Parse(reader.ReadLine());
            reader.Close();
            Buffer = new ReservationBuffer(BufferSize);
            NodeTranslationTable = new NodeTranslationTable(savedFileLocation + ".ntt", cache);
            Cache = NodeTranslationTable;
        }

        #endregion
        #region Public Methods

        public override void Delete(Record record)
        {

            buffer.DeleteEntry(new LeafEntry(record.BoundingBox, record.Address));
            if (buffer.NeedFlush)
                FlushBuffer();
        }
        public override void Insert(Record record)
        {
            Cache.WritePageData(record);
            Buffer.InsertEntry(new LeafEntry(record.BoundingBox, record.Address));
            if (buffer.NeedFlush)
                FlushBuffer();
        }
        public override void SaveIndex(String indexSaveLocation, String cacheSaveLocation, String memorySaveLocation)
        {
            String nodeTranslationTableSaveFileLocation = indexSaveLocation + ".ntt";
            FlushBuffer();
            Cache.SaveCache(cacheSaveLocation, memorySaveLocation);
            NodeTranslationTable.SaveTable(nodeTranslationTableSaveFileLocation);
            StreamWriter writer = new StreamWriter(indexSaveLocation);
            writer.WriteLine(Root);
            writer.WriteLine(TreeHeight);
            writer.WriteLine(Address.NextAddress);
            writer.WriteLine(BufferSize);
            writer.Close();
        }
        public override List<Record> Search(Query query)
        {
            List<Record> results = base.Search(query);
            if (query is RegionQuery)
            {
                List<BufferItem> bufferedOperations = Condense(Buffer.Buffer);
                foreach (BufferItem operation in bufferedOperations)
                {
                    if (operation.Operation == Operation.Insert)
                    {
                        if (query is RangeQuery && Overlaps((RangeQuery)query, operation.Entry.MinimumBoundingBox) ||
                            query is WindowQuery && Overlaps((WindowQuery)query, operation.Entry.MinimumBoundingBox))
                                results.Add(Cache.LookupRecord(operation.Entry.Child));
                    }
                    else if (operation.Operation == Operation.Delete)
                    {
                        Record deletedResult = null;
                        foreach (Record result in results)
                            if (result.Address.Equals(operation.Entry.Child))
                                deletedResult = result;
                        if (deletedResult != null)
                            results.Remove(deletedResult);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            return results;
        }
        public virtual void Flush()
        {
            FlushBuffer();
        }

        #endregion
        #region Protected Methods

        protected override void AdjustTree(Node node1, Node node2)
        {
            if (node1.Address.Equals(Root))
                return;
            if (Root.Equals(Address.Empty))
            {
                Type childType = node1 is Leaf ? typeof(Leaf) : typeof(Node);
                Node rootNode = new Node(Address.Empty, childType);
                Root = rootNode.Address;
                node1.Parent = Root;
                node2.Parent = Root;
                rootNode.AddNodeEntry(new NodeEntry(node1.CalculateMinimumBoundingBox(), node1.Address));
                rootNode.AddNodeEntry(new NodeEntry(node2.CalculateMinimumBoundingBox(), node2.Address));
                Cache.WritePageData(rootNode);
                //Node temp = Cache.LookupNode(rootNode.Address);
                Cache.WritePageData(node1);
                Cache.WritePageData(node2);
                TreeHeight++;
                return;
            }
            Node parent = Cache.LookupNode(node1.Parent);
            NodeEntry entryToUpdate = null;
            foreach (NodeEntry entry in parent.NodeEntries)
                if (entry.Child.Equals(node1.Address))
                    entryToUpdate = entry;
            if (entryToUpdate == null)
            {
                MinimumBoundingBox mbb = node1.CalculateMinimumBoundingBox();
                IndexUnit indexUnit = new IndexUnit(parent.Address, node1.Address, mbb, Operation.Insert);
                NodeTranslationTable.Add(indexUnit);
                parent.AddNodeEntry(new NodeEntry(mbb, node1.Address));
            }
            else
            {
                MinimumBoundingBox mbb = node1.CalculateMinimumBoundingBox();
                IndexUnit indexUnit = new IndexUnit(parent.Address, entryToUpdate.Child, mbb, Operation.Update);
                NodeTranslationTable.Add(indexUnit);
                entryToUpdate.MinimumBoundingBox = mbb;
            }
            if (node2 != null)
            {
                MinimumBoundingBox mbb = node2.CalculateMinimumBoundingBox();
                IndexUnit indexUnit = new IndexUnit(parent.Address, node2.Address, mbb, Operation.Insert);
                NodeTranslationTable.Add(indexUnit);
                parent.AddNodeEntry(new NodeEntry(mbb, node2.Address));
                Cache.WritePageData(node2);
                Cache.WritePageData(node1);                
                if (parent.NodeEntries.Count > Constants.MAXIMUM_ENTRIES_PER_NODE)
                {
                    List<Node> splitNodes = Split(parent);
                    if (parent.Address.Equals(Root))
                        Root = Address.Empty;
                    RemoveFromParent(parent);
                    AdjustTree(splitNodes[0], splitNodes[1]);
                    return;
                }
            }
            AdjustTree(parent, null);
        }
        protected override void CondenseTree(Node node)
        {
            List<Node> eliminatedNodes = new List<Node>();
            while (!node.Address.Equals(Root))
            {
                
                if (node.NodeEntries.Count < Constants.MINIMUM_ENTRIES_PER_NODE)
                {
                    RemoveFromParent(node);
                    eliminatedNodes.Add(node);
                }
                else
                    NodeTranslationTable.Add(new IndexUnit(node.Parent, node.Address, node.CalculateMinimumBoundingBox(), Operation.Update));
                node = Cache.LookupNode(node.Parent);
            }
            foreach(Node eliminatedNode in eliminatedNodes)
            {
                if (eliminatedNode is Leaf)
                    foreach (LeafEntry leafEntry in eliminatedNode.NodeEntries)
                        InsertRecord(new BufferItem(leafEntry, Operation.Insert));
                else
                    foreach (NodeEntry entry in eliminatedNode.NodeEntries)
                        Insert(Cache.LookupNode(entry.Child));
            }
        }
        protected virtual void FlushBuffer()
        {
            foreach (BufferItem operation in Buffer.Buffer)
            {
                if (operation.Operation == Operation.Insert)
                {
                    if (operation.Entry is LeafEntry)
                    {
                        InsertRecord(operation);
                    }
                }
                else if (operation.Operation == Operation.Delete)
                {
                    Record recordToDelete = Cache.LookupRecord(operation.Entry.Child);
                    Leaf leafWithRecord = FindLeaf(recordToDelete, Cache.LookupNode(Root));
                    if (leafWithRecord == null)
                        continue;
                    LeafEntry entryToRemove = null;
                    foreach (LeafEntry entry in leafWithRecord.NodeEntries)
                        if (entry.Child.Equals(recordToDelete.Address))
                            entryToRemove = entry;
                    IndexUnit indexUnit = new IndexUnit(leafWithRecord.Address, operation.Entry.Child, operation.Entry.MinimumBoundingBox, operation.Operation);
                    NodeTranslationTable.Add(indexUnit);
                    leafWithRecord.RemoveNodeEntry(operation.Entry);
                    CondenseTree(leafWithRecord);
                    Node rootNode = Cache.LookupNode(Root);
                    if (rootNode.NodeEntries.Count == 1)
                    {
                        Node newRoot = Cache.LookupNode(rootNode.NodeEntries[0].Child);
                        newRoot.Parent = Address.Empty;
                        Root = newRoot.Address;
                        Cache.DeletePageData(rootNode);
                        Cache.WritePageData(newRoot);
                    }
                    Cache.DeletePageData(recordToDelete);
                }
                else
                    throw new Exception();
            }
            Buffer.Clear();
            NodeTranslationTable.FlushIndexUnits();
        }

        protected virtual void InsertRecord(BufferItem operation)
        {
            Leaf leafToInsertInto = ChooseLeaf(Cache.LookupRecord(operation.Entry.Child));
            IndexUnit indexUnit = new IndexUnit(leafToInsertInto.Address, operation.Entry.Child, operation.Entry.MinimumBoundingBox, operation.Operation);
            NodeTranslationTable.Add(indexUnit);
            leafToInsertInto.AddNodeEntry(operation.Entry);
            if (leafToInsertInto.NodeEntries.Count > Constants.MAXIMUM_ENTRIES_PER_NODE)
            {
                List<Node> splitNodes = Split(leafToInsertInto);
                RemoveFromParent(leafToInsertInto);
                AdjustTree(splitNodes[0], splitNodes[1]);
            }
            else
                AdjustTree(leafToInsertInto);












        }
        protected override void Insert(Node node)
        {
            Node nodeToInsertInto = ChooseNode(node);
            Insert(node, nodeToInsertInto);
            if (nodeToInsertInto.NodeEntries.Count > Constants.MAXIMUM_ENTRIES_PER_NODE)
            {
                List<Node> splitNodes = Split(nodeToInsertInto);
                RemoveFromParent(nodeToInsertInto);
                AdjustTree(splitNodes[0], splitNodes[1]);
            }
            else
                AdjustTree(nodeToInsertInto);

        }
        protected override void Insert(Node newNode, Node node)
        {
            MinimumBoundingBox mbb = newNode.CalculateMinimumBoundingBox();
            IndexUnit indexUnit = new IndexUnit(node.Address, newNode.Address, mbb, Operation.Insert);
            NodeTranslationTable.Add(indexUnit);
            node.AddNodeEntry(new NodeEntry(mbb, newNode.Address));
            newNode.Parent = node.Address;
            Cache.WritePageData(newNode);
        }
        protected override void RemoveFromParent(Node node)
        {
            if (!node.Parent.Equals(Address.Empty))
            {
                Node parent = Cache.LookupNode(node.Parent);
                NodeEntry entryToRemove = null;
                foreach (NodeEntry entry in parent.NodeEntries)
                    if (entry.Child.Equals(node.Address))
                        entryToRemove = entry;
                IndexUnit indexUnit = new IndexUnit(parent.Address, node.Address, entryToRemove.MinimumBoundingBox, Operation.Delete);
                NodeTranslationTable.Add(indexUnit);
            }
            else
                Root = Address.Empty;
            Cache.DeletePageData(node);
        }
        protected override List<Record> Search(KNearestNeighborQuery kNN, Node node)
        {
            return Search(kNN, node, Condense(Buffer.Buffer));
        }
        protected virtual List<Record> Search(KNearestNeighborQuery kNN, Node node, List<BufferItem> changeList)
        {
            PriorityQueue<NodeEntry, Single> proximityQueue = new PriorityQueue<NodeEntry, Single>();
            List<Record> results = new List<Record>(kNN.K);

            List<BufferItem> deletions = new List<BufferItem>();
            foreach(BufferItem change in changeList)
                if(change.Operation == Operation.Delete)
                    deletions.Add(change);
            foreach(BufferItem deletion in deletions)
                changeList.Remove(deletion);
            foreach (BufferItem insertion in changeList)
                proximityQueue.Enqueue(insertion.Entry, GetDistance(kNN.X, kNN.Y, insertion.Entry.MinimumBoundingBox) * -1);

            EnqueNodeEntries(kNN, node, proximityQueue);
            while (results.Count < kNN.K && proximityQueue.Count > 0)
            {
                NodeEntry closestEntry = proximityQueue.Dequeue().Value;
                if (closestEntry is LeafEntry)
                {
                    Boolean entryDeleted = false;
                    foreach (BufferItem deletion in deletions)
                        if (closestEntry.Child.Equals(deletion.Entry.Child))
                            entryDeleted = true;
                    if (!entryDeleted)
                        results.Add(Cache.LookupRecord(closestEntry.Child));
                }
                else
                    EnqueNodeEntries(kNN, Cache.LookupNode(closestEntry.Child), proximityQueue);
            }
            while (results.Contains(null))
                results.Remove(null);
            for(int i = 0; i < results.Count; i++)
                for(int j = i; j < results.Count; j++)
            {
                if (results[i].BoundingBox.MinX == results[j].BoundingBox.MinX &&
                    results[i].BoundingBox.MinY == results[j].BoundingBox.MinY &&
                    results[i].BoundingBox.MaxX == results[j].BoundingBox.MaxX &&
                    results[i].BoundingBox.MaxY == results[j].BoundingBox.MaxY)
                {
                    if (results[i].RecordID.CompareTo(results[j].RecordID) > 0)
                    {
                        Record temp = results[i];
                        results[i] = results[j];
                        results[j] = temp;
                    }
                }
                else
                    break;
            }
            return results;
        }
        protected virtual List<BufferItem> Condense(List<BufferItem> list)
        {
            List<BufferItem> condensed = new List<BufferItem>(),
                expanded = new List<BufferItem>();
            foreach (BufferItem item in list)
                if (item.Operation == Operation.Insert)
                    condensed.Add(item);
                else
                    for (int i = 0; i < condensed.Count; i++)
                        if (condensed[i].Entry.Child.Equals(item.Entry.Child))
                            condensed.RemoveAt(i--);
            return condensed;
        }
        #endregion
    }
}
