using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.Indexes
{
    public class Flash_R_Tree : R_Tree
    {
        #region Instance Variables

        protected NodeTranslationTable nodeTranslationTable;
        protected ReservationBuffer buffer;

        #endregion
        #region Properties

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

        public Flash_R_Tree(Int32 minimumNodeOccupancy, Int32 maximumNodeOccupancy, NodeTranslationTable nodeTranslationTable, ReservationBuffer reservationBuffer)
            : base(minimumNodeOccupancy, maximumNodeOccupancy, nodeTranslationTable)
        {
            Buffer = reservationBuffer;
            NodeTranslationTable = nodeTranslationTable;
        }
        public Flash_R_Tree(String savedFileLocation, NodeTranslationTable nodeTranslationTable, ReservationBuffer reservationBuffer)
            : base(savedFileLocation, nodeTranslationTable)
        {
            Buffer = reservationBuffer;
            NodeTranslationTable = nodeTranslationTable;
        }

        #endregion
        #region Public Methods

        public override void Delete(Record record)
        {
            Leaf leafWithRecord = FindLeaf(record, Cache.LookupNode(Root));
            if (leafWithRecord == null)
                return;
            LeafEntry entryToRemove = null;
            foreach (LeafEntry entry in leafWithRecord.NodeEntries)
                if (entry.Child.Equals(record.Address))
                    entryToRemove = entry;
            buffer.DeleteEntry(entryToRemove);
            if (buffer.NeedFlush)
                FlushBuffer();
            Cache.DeletePageData(record);
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
            throw new Exception("Not implemented");
        }
        public override List<Record> Search(Query query)
        {
            return base.Search(query);
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
                Node rootNode = new Node(MaximumNodeOccupancy, Address.Empty, childType);
                Root = rootNode.Address;
                node1.Parent = Root;
                node2.Parent = Root;
                Cache.WritePageData(rootNode);
                TreeHeight++;
            }
            Node parent = Cache.LookupNode(node1.Parent);
            NodeEntry entryToUpdate = null;
            foreach (NodeEntry entry in parent.NodeEntries)
                if (entry.Child.Equals(node1.Address))
                    entryToUpdate = entry;
            if (entryToUpdate == null)
            {
                MinimumBoundingBox mbb = node1.CalculateMinimumBoundingBox();
                IndexUnit indexUnit = new IndexUnit(parent.Address, entryToUpdate.Child, mbb, Operation.Insert);
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
                parent.AddNodeEntry(new NodeEntry(node2.CalculateMinimumBoundingBox(), node2.Address));
                Cache.WritePageData(node2);
                Cache.WritePageData(node1);                
                if (parent.NodeEntries.Count > MaximumNodeOccupancy)
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
                Node parent = Cache.LookupNode(node.Parent);
                NodeEntry nodeEntry = null;
                foreach (NodeEntry entry in parent.NodeEntries)
                    if (entry.Child.Equals(node.Address))
                        nodeEntry = entry;
                if (node.NodeEntries.Count < minimumNodeOccupancy)
                {
                    IndexUnit indexUnit = new IndexUnit(parent.Address, nodeEntry.Child, nodeEntry.MinimumBoundingBox, Operation.Delete);
                    NodeTranslationTable.Add(indexUnit);
                    parent.RemoveNodeEntry(nodeEntry);
                    eliminatedNodes.Add(node);
                    Cache.DeletePageData(node);
                }
                else
                {
                    nodeEntry.MinimumBoundingBox = node.CalculateMinimumBoundingBox();
                    MinimumBoundingBox mbb = node.CalculateMinimumBoundingBox();
                    IndexUnit indexUnit = new IndexUnit(parent.Address, nodeEntry.Child, nodeEntry.MinimumBoundingBox, Operation.Update);
                    NodeTranslationTable.Add(indexUnit);
                }
                node = parent;
            }
            for (int i = 0; i < eliminatedNodes.Count; i++)
            {
                Node eliminatedNode = eliminatedNodes[i];
                if (eliminatedNode is Leaf)
                    foreach (LeafEntry leafEntry in eliminatedNode.NodeEntries)
                        Insert(Cache.LookupRecord(leafEntry.Child));
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
                        Leaf leafToInsertInto = ChooseLeaf(Cache.LookupRecord(operation.Entry.Child));
                        IndexUnit indexUnit = new IndexUnit(leafToInsertInto.Address, operation.Entry.Child, operation.Entry.MinimumBoundingBox, operation.Operation);
                        NodeTranslationTable.Add(indexUnit);
                        leafToInsertInto.AddNodeEntry(operation.Entry);
                        if (leafToInsertInto.NodeEntries.Count > MaximumNodeOccupancy)
                        {
                            List<Node> splitNodes = Split(leafToInsertInto);
                            RemoveFromParent(leafToInsertInto);
                            AdjustTree(splitNodes[0] as Leaf, splitNodes[1] as Leaf);
                        }
                        else
                            AdjustTree(leafToInsertInto);
                    }
                }
                else if (operation.Operation == Operation.Delete)
                {
                    Node nodeToDeleteFrom = Cache.LookupNode(operation.Entry.Child);
                    IndexUnit indexUnit = new IndexUnit(nodeToDeleteFrom.Address, operation.Entry.Child, operation.Entry.MinimumBoundingBox, operation.Operation);
                    NodeTranslationTable.Add(indexUnit);
                    nodeToDeleteFrom.RemoveNodeEntry(operation.Entry);
                    CondenseTree(nodeToDeleteFrom);
                    Node rootNode = Cache.LookupNode(Root);
                    if (rootNode.NodeEntries.Count == 1)
                    {
                        Node newRoot = Cache.LookupNode(rootNode.NodeEntries[0].Child);
                        newRoot.Parent = Address.Empty;
                        Root = newRoot.Address;
                        Cache.DeletePageData(rootNode);
                        Cache.WritePageData(newRoot);
                    }
                }
                else
                    throw new Exception();
            }
            Buffer.Clear();
            NodeTranslationTable.FlushIndexUnits();
        }
        protected override void Insert(Node node)
        {
            Node nodeToInsertInto = ChooseNode(node);
            Insert(node, nodeToInsertInto);
            if (nodeToInsertInto.NodeEntries.Count > MaximumNodeOccupancy)
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

        #endregion
    }
}
