using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.Indexes
{
    public class R_Tree : Index
    {
        #region Instance Variables

        protected Int32 minimumNodeOccupancy, maximumNodeOccupancy, treeHeight;
        protected Node root;
        protected CacheManager cache;

        #endregion
        #region Properties

        public Int32 MinimumNodeOccupancy
        {
            get { return minimumNodeOccupancy; }
            protected set { minimumNodeOccupancy = value; }
        }
        public Int32 TreeHeight
        {
            get { return treeHeight; }
            protected set { treeHeight = value; }
        }
        public Int32 MaximumNodeOccupancy
        {
            get { return maximumNodeOccupancy; }
            protected set { maximumNodeOccupancy = value; }
        }
        public Node Root
        {
            get { return root; }
            protected set { root = value; }
        }
        public CacheManager Cache
        {
            get { return cache; }
            protected set { cache = value; }
        }

        #endregion
        #region Constructors

        public R_Tree(Int32 minimumNodeOccupancy, Int32 maximumNodeOccupancy, CacheManager cache)
        {
            MinimumNodeOccupancy = minimumNodeOccupancy;
            MaximumNodeOccupancy = maximumNodeOccupancy;
            Cache = cache;
            Root = new Leaf(MaximumNodeOccupancy, Guid.Empty);
            TreeHeight = 1;
        }

        #endregion
        #region Public Methods

        public virtual void Insert(Record record)
        {
            Leaf leafToInsertInto = ChooseLeaf(record);
            Insert(record, leafToInsertInto);
            if (leafToInsertInto.NodeEntries.Count > MaximumNodeOccupancy)
            {
                List<Node> splitNodes = Split(leafToInsertInto);
                RemoveFromParent(leafToInsertInto);
                AdjustTree(splitNodes[0] as Leaf, splitNodes[1] as Leaf);
            }
            else
                AdjustTree(leafToInsertInto);
        }
        public virtual void Delete(Record record)
        {
            Leaf leafWithRecord = FindLeaf(record, Root);
            if (leafWithRecord == null)
                return;
            LeafEntry entryToRemove = null;
            foreach(LeafEntry entry in leafWithRecord.NodeEntries)
                if(entry.Child.Equals(record.Address))
                    entryToRemove = entry;
            leafWithRecord.RemoveNodeEntry(entryToRemove);
            CondenseTree(leafWithRecord);
            if (Root.NodeEntries.Count == 1)
            {
                Node newRoot = Cache.LookupNode(Root.NodeEntries[0].Child);
                newRoot.Parent = Guid.Empty;
                Root = newRoot;
            }
        }
        public virtual void Update(Record originalRecord, Record newRecord)
        {
            Delete(originalRecord);
            Insert(newRecord);
        }
        public virtual List<Record> Search(Query query)
        {
            if (query is RegionQuery)
                return Search(query as RegionQuery, Root);
            else if (query is KNearestNeighborQuery)
                return Search(query as KNearestNeighborQuery, root);
            else
                return null;
        }

        #endregion
        #region Protected Methods

        protected virtual void Insert(Node node)
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
        protected virtual List<Record> Search(RegionQuery window, Node node)
        {
            List<Record> records = new List<Record>();
            foreach (NodeEntry nodeEntry in node.NodeEntries)
            {
                if (window is RangeQuery && Overlaps((RangeQuery)window, nodeEntry.MinimumBoundingBox) ||
                    window is WindowQuery && Overlaps((WindowQuery)window, nodeEntry.MinimumBoundingBox))
                    if (nodeEntry is LeafEntry)
                        records.Add(Cache.LookupRecord(nodeEntry.Child));
                    else
                        records.AddRange(Search(window, Cache.LookupNode(nodeEntry.Child)));
            }
            return records;
        }
        protected virtual List<Record> Search(KNearestNeighborQuery kNN, Node node)
        {
            PriorityQueue<NodeEntry, Double> proximityQueue = new PriorityQueue<NodeEntry, Double>();
            List<Record> results = new List<Record>(kNN.K);
            EnqueNodeEntries(kNN, node, proximityQueue);
            while (results.Count < kNN.K && proximityQueue.Count > 0)
                if (proximityQueue.Peek().Value is LeafEntry)
                    results.Add(Cache.LookupRecord(proximityQueue.Dequeue().Value.Child));
                else
                    EnqueNodeEntries(kNN, Cache.LookupNode(proximityQueue.Dequeue().Value.Child), proximityQueue);
            return results;
        }
        protected virtual Boolean Overlaps(WindowQuery window, MinimumBoundingBox area)
        {
            //checks the only conditions in which they don't overlap
            return !(
                window.MinX > area.MaxX ||
                window.MaxX < area.MinX ||
                window.MinY > area.MaxY ||
                window.MaxY < area.MinY);
        }
        protected virtual Boolean Overlaps(MinimumBoundingBox area1, MinimumBoundingBox area2)
        {
            //checks the only conditions in which they don't overlap
            return !(
                area1.MinX > area2.MaxX ||
                area1.MaxX < area2.MinX ||
                area1.MinY > area2.MaxY ||
                area1.MaxY < area2.MinY);
        }
        protected virtual Boolean Overlaps(RangeQuery range, MinimumBoundingBox area)
        {
            return
                //does not overlap the center point
                !(
                    range.CenterX < area.MinX ||
                    range.CenterX > area.MaxX ||
                    range.CenterY < area.MinY ||
                    range.CenterY > area.MaxY
                )
                ||
                //distance from center to any corner is less than a radius
                (
                    (range.CenterX - area.MinX) * (range.CenterX - area.MinX) + (range.CenterY - area.MinY) * (range.CenterY - area.MinY) < range.Radius * range.Radius ||
                    (range.CenterX - area.MaxX) * (range.CenterX - area.MaxX) + (range.CenterY - area.MinY) * (range.CenterY - area.MinY) < range.Radius * range.Radius ||
                    (range.CenterX - area.MinX) * (range.CenterX - area.MinX) + (range.CenterY - area.MaxY) * (range.CenterY - area.MaxY) < range.Radius * range.Radius ||
                    (range.CenterX - area.MaxX) * (range.CenterX - area.MaxX) + (range.CenterY - area.MaxY) * (range.CenterY - area.MaxY) < range.Radius * range.Radius
                )
                ||
                //the box intersects the circle but no corner lies within the circle
                (
                    (range.CenterX > area.MinX && range.CenterX < area.MaxX && range.CenterY < area.MinY && area.MinY - range.CenterY < range.Radius) ||
                    (range.CenterX > area.MinX && range.CenterX < area.MaxX && range.CenterY > area.MaxY && range.CenterY - area.MaxY < range.Radius) ||
                    (range.CenterY > area.MinY && range.CenterY < area.MaxY && range.CenterX < area.MinX && area.MinX - range.CenterX < range.Radius) ||
                    (range.CenterY > area.MinY && range.CenterY < area.MaxY && range.CenterX > area.MaxX && range.CenterX - area.MaxX < range.Radius)
                );
        }
        protected virtual Leaf ChooseLeaf(Record record)
        {
            Node node = Root;
            while (!(node is Leaf))
            {
                NodeEntry minEnlargment = node.NodeEntries[0];
                Double minEnlargedArea = GetFutureSize(record, minEnlargment.MinimumBoundingBox) - minEnlargment.MinimumBoundingBox.GetArea();
                foreach (NodeEntry nodeEntry in node.NodeEntries)
                {
                    Double enlargment = GetFutureSize(record, nodeEntry.MinimumBoundingBox) - nodeEntry.MinimumBoundingBox.GetArea();
                    if ((enlargment == minEnlargedArea && nodeEntry.MinimumBoundingBox.GetArea() < minEnlargment.MinimumBoundingBox.GetArea()) ||
                        enlargment < minEnlargedArea)
                    {
                        minEnlargedArea = enlargment;
                        minEnlargment = nodeEntry;
                    }
                }
            }
            return node as Leaf;
        }
        protected virtual Node ChooseNode(Node node)
        {
            Node insertionNode = Root;
            MinimumBoundingBox nodeBoundingBox = node.CalculateMinimumBoundingBox();
            Int32 nodeHeight = CalculateHeight(node), currentDepth = 1;
            while (currentDepth + nodeHeight < TreeHeight)//not at right depth
            {
                NodeEntry minEnlargment = insertionNode.NodeEntries[0];
                Double minEnlargedArea = GetFutureSize(nodeBoundingBox, minEnlargment.MinimumBoundingBox) - minEnlargment.MinimumBoundingBox.GetArea();
                foreach (NodeEntry nodeEntry in insertionNode.NodeEntries)
                {
                    Double enlargment = GetFutureSize(nodeBoundingBox, nodeEntry.MinimumBoundingBox) - nodeEntry.MinimumBoundingBox.GetArea();
                    if ((enlargment == minEnlargedArea && nodeEntry.MinimumBoundingBox.GetArea() < minEnlargment.MinimumBoundingBox.GetArea()) ||
                        enlargment < minEnlargedArea)
                    {
                        minEnlargedArea = enlargment;
                        minEnlargment = nodeEntry;
                    }
                }
                currentDepth++;
            }
            return insertionNode;
        }
        protected virtual Int32 CalculateHeight(Node node)
        {
            Int32 height = 2;
            while (node.ChildType != typeof(Leaf))
            {
                node = Cache.LookupNode(node.NodeEntries[0].Child);
                height++;
            }
            return height;
        }
        protected virtual Double GetFutureSize(Record record, MinimumBoundingBox area)
        {
            return GetFutureSize(record.MinimumBoundingBox, area);
        }
        protected virtual Double GetFutureSize(MinimumBoundingBox area1, MinimumBoundingBox area2)
        {
            MinimumBoundingBox futureMinimumBoundingBox = CombineMinimumBoundingBoxes(area1, area2);
            return (futureMinimumBoundingBox.MaxX - futureMinimumBoundingBox.MinX) *
                (futureMinimumBoundingBox.MaxX - futureMinimumBoundingBox.MinX) +
                (futureMinimumBoundingBox.MaxY - futureMinimumBoundingBox.MinY) *
                (futureMinimumBoundingBox.MaxY - futureMinimumBoundingBox.MinY);
        }
        protected virtual MinimumBoundingBox CombineMinimumBoundingBoxes(MinimumBoundingBox area1, MinimumBoundingBox area2)
        {
            Double newMinX, newMaxX, newMinY, newMaxY;
            if (area1.MinX < area2.MinX)
                newMinX = area1.MinX;
            else
                newMinX = area2.MinX;
            if (area1.MaxX > area2.MaxX)
                newMaxX = area1.MaxX;
            else
                newMaxX = area2.MaxX;
            if (area1.MinY < area2.MinY)
                newMinY = area1.MinY;
            else
                newMinY = area2.MinY;
            if (area1.MaxY > area2.MaxY)
                newMaxY = area1.MaxY;
            else
                newMaxY = area2.MaxY;
            return new MinimumBoundingBox(newMinX, newMinY, newMaxX, newMaxY);
        }
        protected virtual void Insert(Record record, Leaf leaf)
        {
            leaf.AddNodeEntry(new LeafEntry(record.MinimumBoundingBox, record.Address));
        }
        protected virtual void Insert(Node newNode, Node node)
        {
            node.AddNodeEntry(new NodeEntry(newNode.CalculateMinimumBoundingBox(), newNode.Address));
        }
        protected virtual List<Node> Split(Node nodeToBeSplit)
        {
            List<NodeEntry> entries = new List<NodeEntry>(nodeToBeSplit.NodeEntries);
            List<NodeEntry> seeds = PickSeeds(entries);
            entries.Remove(seeds[0]);
            entries.Remove(seeds[1]);
            Node node1 = new Node(MaximumNodeOccupancy, nodeToBeSplit.Parent, nodeToBeSplit.ChildType),
                node2 = new Node(MaximumNodeOccupancy, nodeToBeSplit.Parent, nodeToBeSplit.ChildType);
            node1.AddNodeEntry(seeds[0]);
            node2.AddNodeEntry(seeds[1]);
            while (entries.Count > 0)
            {
                if (node1.NodeEntries.Count + entries.Count == MinimumNodeOccupancy)
                {
                    foreach (NodeEntry entry in entries)
                        node1.AddNodeEntry(entry);
                    break;
                }
                else if (node2.NodeEntries.Count + entries.Count == MinimumNodeOccupancy)
                {
                    foreach (NodeEntry entry in entries)
                        node2.AddNodeEntry(entry);
                    break;
                }
                MinimumBoundingBox minimumBoundingBox1 = node1.CalculateMinimumBoundingBox(),
                minimumBoundingBox2 = node2.CalculateMinimumBoundingBox();
                NodeEntry nextEntry = PickNext(entries, minimumBoundingBox1, minimumBoundingBox2);
                if (GetFutureSize(nextEntry.MinimumBoundingBox, minimumBoundingBox1) ==
                    GetFutureSize(nextEntry.MinimumBoundingBox, minimumBoundingBox2))
                {
                    if (minimumBoundingBox1.GetArea() == minimumBoundingBox2.GetArea())
                        if (node1.NodeEntries.Count <= node2.NodeEntries.Count)
                            node1.AddNodeEntry(nextEntry);
                        else
                            node2.AddNodeEntry(nextEntry);
                    else if (minimumBoundingBox1.GetArea() < minimumBoundingBox2.GetArea())
                        node1.AddNodeEntry(nextEntry);
                    else
                        node2.AddNodeEntry(nextEntry);
                }
                else if (GetFutureSize(nextEntry.MinimumBoundingBox, minimumBoundingBox1) <
                    GetFutureSize(nextEntry.MinimumBoundingBox, minimumBoundingBox2))
                    node1.AddNodeEntry(nextEntry);
                else
                    node2.AddNodeEntry(nextEntry);
            }
            List<Node> newNodes = new List<Node>();
            newNodes.Add(node1);
            newNodes.Add(node2);
            return newNodes;
        }
        protected virtual List<NodeEntry> PickSeeds(List<NodeEntry> seedPool)
        {
            NodeEntry worstPairEntry1, worstPairEntry2;
            worstPairEntry1 = seedPool[0];
            worstPairEntry2 = seedPool[1];
            Double worstEnlargement = GetFutureSize(worstPairEntry1.MinimumBoundingBox, worstPairEntry2.MinimumBoundingBox) -
                        worstPairEntry1.MinimumBoundingBox.GetArea() - worstPairEntry2.MinimumBoundingBox.GetArea();
            for (int i = 0; i < seedPool.Count; i++)
                for (int j = i + 1; j < seedPool.Count; j++)
                {
                    Double d = GetFutureSize(seedPool[i].MinimumBoundingBox, seedPool[j].MinimumBoundingBox) -
                        seedPool[i].MinimumBoundingBox.GetArea() - seedPool[j].MinimumBoundingBox.GetArea();
                    if(d > worstEnlargement)
                    {
                        worstPairEntry1 = seedPool[i];
                        worstPairEntry2 = seedPool[j];
                        worstEnlargement = d;
                    }
                }
            List<NodeEntry> worstPair = new List<NodeEntry>();
            worstPair.Add(worstPairEntry1);
            worstPair.Add(worstPairEntry2);
            return worstPair;
        }
        protected virtual NodeEntry PickNext(List<NodeEntry> entryPool, MinimumBoundingBox minimumBoundingBox1, MinimumBoundingBox minimumBoundingBox2)
        {
            NodeEntry nextEntry = entryPool[0];
            
            Double maxEnlargementDifference = Math.Abs(
                GetFutureSize(nextEntry.MinimumBoundingBox, minimumBoundingBox1) -
                GetFutureSize(nextEntry.MinimumBoundingBox, minimumBoundingBox2));
            foreach (NodeEntry entry in entryPool)
            {
                Double enlargmentDifference = Math.Abs(
                GetFutureSize(entry.MinimumBoundingBox, minimumBoundingBox1) -
                GetFutureSize(entry.MinimumBoundingBox, minimumBoundingBox2));
                if (enlargmentDifference > maxEnlargementDifference)
                {
                    maxEnlargementDifference = enlargmentDifference;
                    nextEntry = entry;
                }
            }
            return nextEntry;
        }
        protected virtual void AdjustTree(Node node)
        {
            AdjustTree(node, null);
        }
        protected virtual void AdjustTree(Node node1, Node node2)
        {
            if (node1 == Root)
                return;
            if (Root == null)
            {
                Root = new Node(MaximumNodeOccupancy, Guid.Empty, typeof(Node));
                Root.AddNodeEntry(new NodeEntry(node1.CalculateMinimumBoundingBox(), node1.Address));
                Root.AddNodeEntry(new NodeEntry(node2.CalculateMinimumBoundingBox(), node2.Address));
                node1.Parent = Root.Address;
                node2.Parent = Root.Address;
                TreeHeight++;
            }            
            Node parent = Cache.LookupNode(node1.Parent);
            parent.AddNodeEntry(new NodeEntry(node1.CalculateMinimumBoundingBox(), node1.Address));
            if (node2 != null)
            {
                NodeEntry newEntry = new NodeEntry(node2.CalculateMinimumBoundingBox(), node2.Address);
                parent.AddNodeEntry(newEntry);
                if (parent.NodeEntries.Count >= MaximumNodeOccupancy)
                {
                    List<Node> splitNodes = Split(parent);
                    if (parent == Root)
                        Root = null;
                    RemoveFromParent(parent);
                    AdjustTree(splitNodes[0], splitNodes[1]);
                    return;
                }
            }
            AdjustTree(node1, null);
        }
        protected virtual Leaf FindLeaf(Record record, Node node)
        {
            if (node is Leaf)
            {
                foreach (LeafEntry entry in node.NodeEntries)
                    if (entry.Child.Equals(record.Address))
                        return node as Leaf;
            }
            else
                foreach (NodeEntry entry in node.NodeEntries)
                    if (Overlaps(entry.MinimumBoundingBox, record.MinimumBoundingBox))
                    {
                        Leaf leaf = FindLeaf(record, Cache.LookupNode(entry.Child));
                        if (leaf != null)
                            return leaf;
                    }
            return null;
        }
        protected virtual void CondenseTree(Node node)
        {
            List<Node> eliminatedNodes = new List<Node>();
            while(node != root)
            {
                Node parent = Cache.LookupNode(node.Parent);
                NodeEntry nodeEntry = null;
                foreach (NodeEntry entry in parent.NodeEntries)
                    if (entry.Child.Equals(node.Address))
                        nodeEntry = entry;
                if (node.NodeEntries.Count < minimumNodeOccupancy)
                {
                    parent.RemoveNodeEntry(nodeEntry);
                    eliminatedNodes.Add(node);
                }
                else
                    nodeEntry.MinimumBoundingBox = node.CalculateMinimumBoundingBox();
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
        protected virtual void RemoveFromParent(Node node)
        {
            if (node.Parent.Equals(Guid.Empty))
                return;
            Node parent = Cache.LookupNode(node.Parent);
            NodeEntry entryToRemove = null;
            foreach (NodeEntry entry in parent.NodeEntries)
                if (entry.Child.Equals(node.Address))
                    entryToRemove = entry;
            parent.RemoveNodeEntry(entryToRemove);
        }
        protected virtual Double GetDistance(Double x, Double y, MinimumBoundingBox area)
        {
            if (area.MaxX < x && area.MinY > y)
                return GetDistance(x, y, area.MaxX, area.MinY);
            else if (area.MinX > x && area.MinY > y)
                return GetDistance(x, y, area.MinX, area.MinY);
            else if (area.MaxX < x && area.MaxY < y)
                return GetDistance(x, y, area.MaxX, area.MaxY);
            else if (area.MinX > x && area.MaxY < y)
                return GetDistance(x, y, area.MinX, area.MaxY);
            else if (area.MaxX < x)
                return GetDistance(x, y, area.MaxX, y);
            else if (area.MinX > x)
                return GetDistance(x, y, area.MinX, y);
            else if (area.MaxY < y)
                return GetDistance(x, y, x, area.MaxY);
            else if (area.MinY > y)
                return GetDistance(x, y, x, area.MinY);
            else
                return 0;
        }
        protected virtual Double GetDistance(Double x1, Double y1, Double x2, Double y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }
        protected virtual void EnqueNodeEntries(KNearestNeighborQuery kNN, Node node, PriorityQueue<NodeEntry, Double> proximityQueue)
        {
            foreach (NodeEntry entry in node.NodeEntries)
                proximityQueue.Enqueue(entry, GetDistance(kNN.X, kNN.Y, entry.MinimumBoundingBox));
        }

        #endregion
    }
}
