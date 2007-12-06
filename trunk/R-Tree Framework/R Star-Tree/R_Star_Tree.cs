using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.Indexes
{
    public class R_Star_Tree : R_Tree
    {
        #region Instance Variables

        protected List<Int32> overflowMarkers;

        #endregion
        #region Properties

        protected Int32 NumberOfEntriesForReInsert
        {
            get { return MaximumNodeOccupancy * 3 / 10 + ((MaximumNodeOccupancy * 3) % 10 > 4 ? 1 : 0); }
        }
        protected List<Int32> OverflowMarkers
        {
            get { return overflowMarkers; }
            set { overflowMarkers = value; }
        }

        #endregion
        #region Constructors

        public R_Star_Tree(Int32 minimumNodeOccupancy, Int32 maximumNodeOccupancy, CacheManager cache)
            : base(minimumNodeOccupancy, maximumNodeOccupancy, cache)
        {
        }

        #endregion
        #region Public Methods

        public override void Insert(Record record)
        {
            OverflowMarkers = new List<Int32>();
            LeafEntry newEntry = new LeafEntry(record.BoundingBox, record.Address);
            Insert(newEntry, TreeHeight);
        }

        #endregion
        #region Protected Methods

        #region Finished

        protected virtual Single GetFutureOverlap(NodeEntry entry, NodeEntry insertionEntry, Node node)
        {
            MinimumBoundingBox overlapMinimumBoundingBox =
                CombineMinimumBoundingBoxes(entry.MinimumBoundingBox, insertionEntry.MinimumBoundingBox);
            return GetOverlap(insertionEntry, overlapMinimumBoundingBox, node);
        }
        protected virtual Single GetOverlap(NodeEntry insertionEntry, Node node)
        {
            return GetOverlap(insertionEntry, insertionEntry.MinimumBoundingBox, node);
        }
        protected virtual Single GetOverlap(NodeEntry insertionEntry, MinimumBoundingBox overlapMinimumBoundingBox, Node node)
        {
            foreach (NodeEntry nodeEntry in node.NodeEntries)
                if (nodeEntry != insertionEntry)
                    overlapMinimumBoundingBox = IntersectMinimumBoundingBoxes(overlapMinimumBoundingBox, nodeEntry.MinimumBoundingBox);
            return
                (overlapMinimumBoundingBox.MaxX - overlapMinimumBoundingBox.MinX) *
                (overlapMinimumBoundingBox.MaxY - overlapMinimumBoundingBox.MinY);
        }
        protected virtual MinimumBoundingBox IntersectMinimumBoundingBoxes(MinimumBoundingBox area1, MinimumBoundingBox area2)
        {
            if (!Overlaps(area1, area2))
                return new MinimumBoundingBox(0, 0, 0, 0);
            Single newMinX, newMaxX, newMinY, newMaxY;
            if (area1.MinX > area2.MinX)
                newMinX = area1.MinX;
            else
                newMinX = area2.MinX;
            if (area1.MaxX < area2.MaxX)
                newMaxX = area1.MaxX;
            else
                newMaxX = area2.MaxX;
            if (area1.MinY > area2.MinY)
                newMinY = area1.MinY;
            else
                newMinY = area2.MinY;
            if (area1.MaxY < area2.MaxY)
                newMaxY = area1.MaxY;
            else
                newMaxY = area2.MaxY;
            return new MinimumBoundingBox(newMinX, newMinY, newMaxX, newMaxY);
        }
        protected virtual Single GetMargin(Node node1, Node node2)
        {
            return GetMargin(node1) + GetMargin(node2);
        }
        protected virtual Single GetMargin(Node node)
        {
            MinimumBoundingBox box = node.CalculateMinimumBoundingBox();
            return (box.MaxX - box.MinX) + (box.MaxY - box.MinY);
        }
        protected virtual Single GetOverlap(Node node1, Node node2)
        {
            MinimumBoundingBox overlapArea =
                IntersectMinimumBoundingBoxes(node1.CalculateMinimumBoundingBox(), node2.CalculateMinimumBoundingBox());
            return
                (overlapArea.MaxX - overlapArea.MinX) *
                (overlapArea.MaxY - overlapArea.MinY);
        }
        protected virtual Single GetCenterDistance(MinimumBoundingBox box1, MinimumBoundingBox box2)
        {
            return GetDistance((box1.MaxX + box1.MinX)/2, (box1.MaxY + box1.MinY) / 2, (box2.MaxX + box2.MinX) / 2, (box2.MaxY + box2.MinY) / 2);
        }

        #endregion

        protected override Node ChooseLeaf(Record record)
        {
            LeafEntry entry = new LeafEntry(record.BoundingBox, record.Address);
            ChooseNode(entry, TreeHeight);
        }
        protected override Node ChooseNode(Node node)
        {
            ChooseNode(node, TreeHeight - CalculateHeight(node) + 1);
        }
        protected virtual Node ChooseNode(NodeEntry entry, Int32 targetLevel)
        {
            Node insertionNode = Cache.LookupNode(Root);
            Int32 currentLevel = 1;
            while (!(insertionNode is Leaf) && currentLevel < level)
            {
                if (insertionNode.ChildType.Equals(typeof(Leaf)))
                {
                    NodeEntry minOverlap = insertionNode.NodeEntries[0];
                    Single minOverlapIncrease = GetFutureOverlap(entry, minOverlap, insertionNode) -
                        GetOverlap(minOverlap, insertionNode);
                    foreach (NodeEntry nodeEntry in insertionNode.NodeEntries)
                    {
                        Single overlapIncrease = GetFutureOverlap(entry, nodeEntry, insertionNode) -
                        GetOverlap(nodeEntry, insertionNode);
                        if (overlapIncrease == minOverlapIncrease)
                        {
                            Single nodeEntryFutureSize = GetFutureSize(entry.MinimumBoundingBox, nodeEntry.MinimumBoundingBox),
                            nodeEntrySize = nodeEntry.MinimumBoundingBox.GetArea(),
                            minOverlapFutureSize = GetFutureSize(entry.MinimumBoundingBox, minOverlap.MinimumBoundingBox),
                            minOverlapSize = minOverlap.MinimumBoundingBox.GetArea();
                            if ((nodeEntryFutureSize - nodeEntrySize == minOverlapFutureSize - minOverlapSize &&
                                nodeEntryFutureSize < minOverlapFutureSize
                                ) ||
                                nodeEntryFutureSize - nodeEntrySize < minOverlapFutureSize - minOverlapSize)
                            {
                                minOverlapIncrease = overlapIncrease;
                                minOverlap = nodeEntry;
                            }
                        }
                        else if (overlap < minOverlapArea)
                        {
                            minOverlapIncrease = overlapIncrease;
                            minOverlap = nodeEntry;
                        }
                    }
                    insertionNode = Cache.LookupNode(minOverlap.Child);
                }
                else
                {
                    NodeEntry minEnlagement = insertionNode.NodeEntries[0];
                    Single minAreaIncrease = GetFutureSize(entry.MinimumBoundingBox, minEnlagement.MinimumBoundingBox) -
                                minEnlagement.MinimumBoundingBox.GetArea();
                    foreach (NodeEntry nodeEntry in insertionNode.NodeEntries)
                    {
                        Single nodeEntryFutureSize = GetFutureSize(entry.MinimumBoundingBox, nodeEntry.MinimumBoundingBox),
                                nodeEntrySize = nodeEntry.MinimumBoundingBox.GetArea(),
                                minEnlargementFutureSize = GetFutureSize(entry.MinimumBoundingBox, minEnlagement.MinimumBoundingBox),
                                minEnlargementSize = minEnlagement.MinimumBoundingBox.GetArea();
                        if ((nodeEntryFutureSize - nodeEntrySize == minEnlargementFutureSize - minEnlargementSize &&
                            nodeEntryFutureSize < minEnlargementFutureSize
                            ) ||
                            nodeEntryFutureSize - nodeEntrySize < minEnlargementFutureSize - minEnlargementSize)
                        {
                            minAreaIncrease = nodeEntryFutureSize - nodeEntrySize;
                            minEnlagement = nodeEntry;
                        }
                    }
                    insertionNode = Cache.LookupNode(minEnlagement.Child);
                }
            }
            return insertionNode;
        }
        protected override List<Node> Split(Node nodeToBeSplit)
        {
            //
            //Determine Axis
            //


            //Sort each axis by upper and lower corners
            List<NodeEntry> xLowerSorted = new List<NodeEntry>(nodeToBeSplit.NodeEntries),
                xUpperSorted = new List<NodeEntry>(nodeToBeSplit.NodeEntries),
                yLowerSorted = new List<NodeEntry>(nodeToBeSplit.NodeEntries),
                yUpperSorted = new List<NodeEntry>(nodeToBeSplit.NodeEntries);

            xLowerSorted.Sort(new NodeEntryLowerAxisComparerX());
            xUpperSorted.Sort(new NodeEntryUpperAxisComparerX());
            yLowerSorted.Sort(new NodeEntryLowerAxisComparerY());
            yUpperSorted.Sort(new NodeEntryUpperAxisComparerY());

            //Generate Distributions
            List<Pair<Node, Node>> xLowerDistributions = new List<Pair<Node, Node>>(),
                xUpperDistributions = new List<Pair<Node, Node>>(),
                yLowerDistributions = new List<Pair<Node, Node>>(),
                yUpperDistributions = new List<Pair<Node, Node>>();

            Dictionary<List<NodeEntry>, List<Pair<Node, Node>>> axii =
                new Dictionary<List<NodeEntry>, List<Pair<Node, Node>>>();

            axii.Add(xLowerSorted, xLowerDistributions);
            axii.Add(xUpperSorted, xUpperDistributions);
            axii.Add(yLowerSorted, yLowerDistributions);
            axii.Add(yUpperSorted, yUpperDistributions);

            foreach (KeyValuePair<List<NodeEntry>, List<Pair<Node, Node>>> axis in axii)
                for (int i = 0; i < MaximumNodeOccupancy - 2 * MinimumNodeOccupancy + 2; i++)
                {
                    Node group1, group2;
                    if (nodeToBeSplit is Leaf)
                    {
                        group1 = new Leaf(maximumNodeOccupancy, nodeToBeSplit.Parent);
                        group2 = new Leaf(maximumNodeOccupancy, nodeToBeSplit.Parent);
                    }
                    else
                    {
                        group1 = new Node(maximumNodeOccupancy, nodeToBeSplit.Parent, nodeToBeSplit.ChildType);
                        group2 = new Node(maximumNodeOccupancy, nodeToBeSplit.Parent, nodeToBeSplit.ChildType);
                    }
                    foreach (NodeEntry entry in axis.Key.GetRange(0, MinimumNodeOccupancy + i))
                        group1.AddNodeEntry(entry);
                    foreach (NodeEntry entry in axis.Key.GetRange(MinimumNodeOccupancy + i, axis.Key.Count - (MinimumNodeOccupancy + i)))
                        group2.AddNodeEntry(entry);
                    axis.Value.Add(new Pair<Node, Node>(group1, group2));
                }

            //Sum margins
            Single xLowerMarginSum = 0, xUpperMarginSum = 0, yLowerMarginSum = 0, yUpperMarginSum = 0;
            foreach (Pair<Node, Node> distribution in xLowerDistributions)
                xLowerMarginSum += GetMargin(distribution.Value1, distribution.Value2);
            foreach (Pair<Node, Node> distribution in xUpperDistributions)
                xUpperMarginSum += GetMargin(distribution.Value1, distribution.Value2);
            foreach (Pair<Node, Node> distribution in yLowerDistributions)
                yLowerMarginSum += GetMargin(distribution.Value1, distribution.Value2);
            foreach (Pair<Node, Node> distribution in yUpperDistributions)
                yUpperMarginSum += GetMargin(distribution.Value1, distribution.Value2);

            //Choose Axis
            List<Pair<Node, Node>> chosenAxis;
            if (xLowerMarginSum <= xUpperMarginSum && xLowerMarginSum <= yLowerMarginSum && xLowerMarginSum <= yUpperMarginSum)
                chosenAxis = xLowerDistributions;
            else if (xUpperMarginSum <= xLowerMarginSum && xUpperMarginSum <= yLowerMarginSum && xUpperMarginSum <= yUpperMarginSum)
                chosenAxis = xUpperDistributions;
            else if (yLowerMarginSum <= xUpperMarginSum && yLowerMarginSum <= xLowerMarginSum && yLowerMarginSum <= yUpperMarginSum)
                chosenAxis = yLowerDistributions;
            else
                chosenAxis = yUpperDistributions;

            //
            //Determine Distribution
            //


            Pair<Node, Node> chosenDistribution = chosenAxis[0];
            Single minimumOverlapArea = GetOverlap(chosenDistribution.Value1, chosenDistribution.Value2);
            foreach (Pair<Node, Node> distribution in chosenAxis)
            {
                Single overlapArea = GetOverlap(distribution.Value1, distribution.Value2);
                if ((overlapArea == minimumOverlapArea &&
                    GetFutureSize(distribution.Value1.CalculateMinimumBoundingBox(), distribution.Value2.CalculateMinimumBoundingBox()) < 
                    GetFutureSize(chosenDistribution.Value1.CalculateMinimumBoundingBox(), chosenDistribution.Value2.CalculateMinimumBoundingBox())
                    ) ||
                    overlapArea < minimumOverlapArea)
                {
                    chosenDistribution = distribution;
                    minimumOverlapArea = overlapArea;
                }
            }

            //
            //Distribute
            //


            List<Node> newNodes = new List<Node>();
            newNodes.Add(chosenDistribution.Value1);
            newNodes.Add(chosenDistribution.Value2);
            foreach (Node newNode in newNodes)
            {
                if (!(newNode is Leaf))
                    foreach (NodeEntry entry in newNode.NodeEntries)
                    {
                        Node child = Cache.LookupNode(entry.Child);
                        child.Parent = newNode.Address;
                        Cache.WritePageData(child);
                    }
                Cache.WritePageData(newNode);
            }
            return newNodes;
        }
        protected virtual void Insert(NodeEntry entry, Int32 level)
        {
            Node nodeToInsertInto = ChooseNode(entry, level);
            nodeToInsertInto.AddNodeEntry(entry);
            if (nodeToInsertInto.NodeEntries.Count > MaximumNodeOccupancy)
            {
                List<Node> splitNodes = OverFlowTreatment(level, nodeToInsertInto);
                if (splitNodes != null)
                {
                    RemoveFromParent(nodeToInsertInto);
                    AdjustTree(splitNodes[0], splitNodes[1]);
                    return;
                }
            }
            AdjustTree(nodeToInsertInto);
        }
        protected virtual List<Node> OverFlowTreatment(Node nodeToInsertInto, Int32 level)
        {
            if (nodeToInsertInto.Address.Equals(Root) || OverflowMarkers.Contains(level))
            {
                return Split(nodeToInsertInto);
            }
            else
            {
                OverflowMarkers.Add(level);
                ReInsert(nodeToInsertInto, level);
                return null;
            }
        }

        protected virtual void ReInsert(Node node, Int32 level)
        {
            MinimumBoundingBox nodeBox = node.CalculateMinimumBoundingBox();
            SortedList<Single, NodeEntry> distances = new SortedList<Single, NodeEntry>(),
            reInsertions = new SortedList<Single, NodeEntry>(); ;
            foreach (NodeEntry entry in node.NodeEntries)
                distances.Add(GetCenterDistance(nodeBox, entry.MinimumBoundingBox) * -1, entry);
            for (int i = 0; i < NumberOfEntriesForReInsert; i++)
                reInsertions.Add(distances.Keys[i] * -1, distances[distances.Keys[i]]);
            foreach (NodeEntry entry in reInsertions.Values)
                node.RemoveNodeEntry(entry);
            AdjustTree(node);
            foreach (NodeEntry entry in reInsertions.Values)
                Insert(entry, level);
        }

        protected virtual void AdjustTree(Node node1, Node node2)
        {
            if (node1.Address.Equals(Root))
            {
                Cache.WritePageData(node1);
                return;
            }
            if (Root.Equals(Address.Empty))
            {
                Node rootNode = new Node(MaximumNodeOccupancy, Address.Empty, typeof(Node));
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
                parent.AddNodeEntry(new NodeEntry(node1.CalculateMinimumBoundingBox(), node1.Address));
            else
                entryToUpdate.MinimumBoundingBox = node1.CalculateMinimumBoundingBox();
            Cache.WritePageData(node1);
            if (node2 != null)
            {
                parent.AddNodeEntry(new NodeEntry(node2.CalculateMinimumBoundingBox(), node2.Address));
                Cache.WritePageData(node2);
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

        #endregion
    }
}
