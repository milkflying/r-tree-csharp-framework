using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.Indexes
{
    public class R_Star_Tree : R_Tree
    {
        protected List<Int32> overflowMarkers;

        protected List<Int32> OverflowMarkers
        {
            get { return overflowMarkers; }
            set { overflowMarkers = value; }
        }
        protected Int32 NumberOfEntriesForReInsert
        {
            get { return 3 * MaximumNodeOccupancy / 10 + ((3 * MaximumNodeOccupancy) % 10 > 4 ? 1 : 0); }
        }

        public R_Star_Tree(Int32 minimumNodeOccupancy, Int32 maximumNodeOccupancy, CacheManager cache)
            : base(minimumNodeOccupancy, maximumNodeOccupancy, cache)
        {
        }

        public override void Insert(Record record)
        {
            OverflowMarkers = new List<Int32>();
            LeafEntry newEntry = new LeafEntry(record.MinimumBoundingBox, record.Address);
            Insert(newEntry, TreeHeight);
        }

        protected virtual void Insert(NodeEntry entry, Int32 level)
        {
            Node nodeToInsertInto = ChooseNode(entry, level);
            nodeToInsertInto.AddNodeEntry(entry);
            if (nodeToInsertInto.NodeEntries.Count > MaximumNodeOccupancy)
            {
                if (OverflowMarkers.Contains(level))
                {
                    List<Node> splitNodes = Split(nodeToInsertInto);
                    RemoveFromParent(nodeToInsertInto);
                    AdjustTree(splitNodes[0], splitNodes[1], level);
                }
                else
                {
                    OverflowMarkers.Add(level);
                    ReInsert(nodeToInsertInto);
                    AdjustTree(nodeToInsertInto, level);
                }
            }
            else
                AdjustTree(nodeToInsertInto, level);
        }
        protected override Leaf ChooseLeaf(Record record)
        {
            return ChooseNode(new LeafEntry(record.MinimumBoundingBox, record.Address), TreeHeight) as Leaf;
        }
        protected virtual Node ChooseNode(NodeEntry entry, Int32 level)
        {
            Node insertionNode = Root;
            Int32 currentDepth = 1;
            while (currentDepth < level)
            {
                if (insertionNode.ChildType.Equals(typeof(Leaf)))
                {
                    Record record;
                    NodeEntry minOverlap = insertionNode.NodeEntries[0];
                    Double minOverlapArea = GetFutureOverlap(entry, minOverlap, insertionNode);
                    foreach (NodeEntry nodeEntry in insertionNode.NodeEntries)
                    {
                        Double overlap = GetFutureOverlap(entry, nodeEntry, insertionNode);
                        if (overlap == minOverlapArea)
                        {
                            Double nodeEntryFutureSize = GetFutureSize(entry.MinimumBoundingBox, nodeEntry.MinimumBoundingBox),
                                nodeEntrySize = nodeEntry.MinimumBoundingBox.GetArea(),
                                minOverlapFutureSize = GetFutureSize(entry.MinimumBoundingBox, minOverlap.MinimumBoundingBox),
                                minOverlapSize = minOverlap.MinimumBoundingBox.GetArea();

                            if ((nodeEntryFutureSize - nodeEntrySize == minOverlapFutureSize - minOverlapSize &&
                                nodeEntryFutureSize < minOverlapFutureSize
                                ) ||
                                nodeEntryFutureSize - nodeEntrySize < minOverlapFutureSize - minOverlapSize)
                            {
                                minOverlapArea = overlap;
                                minOverlap = nodeEntry;
                            }
                        }
                        else if (overlap < minOverlapArea)
                        {
                            minOverlapArea = overlap;
                            minOverlap = nodeEntry;
                        }
                    }
                    insertionNode = Cache.LookupNode(minOverlap.Child);
                }
                else
                {
                    NodeEntry minEnlargment = insertionNode.NodeEntries[0];
                    Double minEnlargedArea = GetFutureSize(entry.MinimumBoundingBox, minEnlargment.MinimumBoundingBox) - minEnlargment.MinimumBoundingBox.GetArea();
                    foreach (NodeEntry nodeEntry in insertionNode.NodeEntries)
                    {
                        Double enlargment = GetFutureSize(entry.MinimumBoundingBox, nodeEntry.MinimumBoundingBox) - nodeEntry.MinimumBoundingBox.GetArea();
                        if ((enlargment == minEnlargedArea && nodeEntry.MinimumBoundingBox.GetArea() < minEnlargment.MinimumBoundingBox.GetArea()) ||
                            enlargment < minEnlargedArea)
                        {
                            minEnlargedArea = enlargment;
                            minEnlargment = nodeEntry;
                        }
                    }
                    insertionNode = Cache.LookupNode(minEnlargment.Child);
                }
                currentDepth++;
            }
            return insertionNode;
        }
        protected override List<Node> Split(Node nodeToBeSplit)
        {
            //Determine Axis
            List<NodeEntry> xLowerSorted = new List<NodeEntry>(nodeToBeSplit.NodeEntries),
                xUpperSorted = new List<NodeEntry>(nodeToBeSplit.NodeEntries),
                yLowerSorted = new List<NodeEntry>(nodeToBeSplit.NodeEntries),
                yUpperSorted = new List<NodeEntry>(nodeToBeSplit.NodeEntries);
            xLowerSorted.Sort(new NodeEntryLowerAxisComparerX());
            xUpperSorted.Sort(new NodeEntryUpperAxisComparerX());
            yLowerSorted.Sort(new NodeEntryLowerAxisComparerY());
            yUpperSorted.Sort(new NodeEntryUpperAxisComparerY());

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

            Double xLowerMarginSum = 0, xUpperMarginSum = 0, yLowerMarginSum = 0, yUpperMarginSum = 0;

            foreach (KeyValuePair<List<NodeEntry>, List<Pair<Node, Node>>> axis in axii)
                for (int i = 1; i < maximumNodeOccupancy - 2 * minimumNodeOccupancy + 2 + 1; i++)
                {
                    Node group1 = new Node(maximumNodeOccupancy, Guid.Empty, typeof(NodeEntry)),
                        group2 = new Node(maximumNodeOccupancy, Guid.Empty, typeof(NodeEntry));
                    group1.NodeEntries.AddRange(axis.Key.GetRange(1, i));
                    group2.NodeEntries.AddRange(axis.Key.GetRange(i, axis.Key.Count - i));
                    axis.Value.Add(new Pair<Node, Node>(group1, group2));
                }
            foreach (Pair<Node, Node> distribution in xLowerDistributions)
                xLowerMarginSum += GetMargin(distribution.Value1, distribution.Value2);
            foreach (Pair<Node, Node> distribution in xUpperDistributions)
                xUpperMarginSum += GetMargin(distribution.Value1, distribution.Value2);
            foreach (Pair<Node, Node> distribution in yLowerDistributions)
                yLowerMarginSum += GetMargin(distribution.Value1, distribution.Value2);
            foreach (Pair<Node, Node> distribution in yUpperDistributions)
                yUpperMarginSum += GetMargin(distribution.Value1, distribution.Value2);

            List<Pair<Node, Node>> chosenAxis;
            if (xLowerMarginSum <= xUpperMarginSum && xLowerMarginSum <= yLowerMarginSum && xLowerMarginSum <= yUpperMarginSum)
                chosenAxis = xLowerDistributions;
            else if (xUpperMarginSum <= xLowerMarginSum && xUpperMarginSum <= yLowerMarginSum && xUpperMarginSum <= yUpperMarginSum)
                chosenAxis = xUpperDistributions;
            else if (yLowerMarginSum <= xUpperMarginSum && yLowerMarginSum <= xLowerMarginSum && yLowerMarginSum <= yUpperMarginSum)
                chosenAxis = yLowerDistributions;
            else
                chosenAxis = yUpperDistributions;

            //Determine Distribution
            Pair<Node, Node> chosenDistribution = chosenAxis[0];
            Double minimumOverlapArea = GetOverlap(chosenDistribution.Value1, chosenDistribution.Value2);

            foreach (Pair<Node, Node> distribution in chosenAxis)
            {
                Double overlapArea = GetOverlap(distribution.Value1, distribution.Value2);
                if ((overlapArea == minimumOverlapArea &&
                    GetFutureSize(chosenDistribution.Value1.CalculateMinimumBoundingBox(), chosenDistribution.Value2.CalculateMinimumBoundingBox()) <
                    GetFutureSize(distribution.Value1.CalculateMinimumBoundingBox(), distribution.Value2.CalculateMinimumBoundingBox())
                    ) ||
                    overlapArea < minimumOverlapArea)
                {
                    chosenDistribution = distribution;
                    minimumOverlapArea = overlapArea;
                }
            }

            //Distribute
            List<Node> newNodes = new List<Node>();
            newNodes.Add(chosenDistribution.Value1);
            newNodes.Add(chosenDistribution.Value2);
            return newNodes;
        }
        protected virtual Double GetFutureOverlap(NodeEntry entry, NodeEntry insertionEntry, Node node)
        {
            MinimumBoundingBox overlapMinimumBoundingBox =
                CombineMinimumBoundingBoxes(entry.MinimumBoundingBox, insertionEntry.MinimumBoundingBox);
            foreach (NodeEntry nodeEntry in node.NodeEntries)
                if (nodeEntry != insertionEntry)
                    overlapMinimumBoundingBox = IntersectMinimumBoundingBoxes(overlapMinimumBoundingBox, nodeEntry.MinimumBoundingBox);
            return (overlapMinimumBoundingBox.MaxX - overlapMinimumBoundingBox.MinX) *
                (overlapMinimumBoundingBox.MaxY - overlapMinimumBoundingBox.MinY);
        }
        protected virtual MinimumBoundingBox IntersectMinimumBoundingBoxes(MinimumBoundingBox area1, MinimumBoundingBox area2)
        {
            if (!Overlaps(area1, area2))
                return new MinimumBoundingBox(0, 0, 0, 0);
            Double newMinX, newMaxX, newMinY, newMaxY;
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
        protected virtual Double GetMargin(Node node1, Node node2)
        {
            Double margin = 0;
            MinimumBoundingBox box1 = node1.CalculateMinimumBoundingBox(),
                box2 = node2.CalculateMinimumBoundingBox();
            margin += (box1.MaxX - box1.MinX);
            margin += (box2.MaxX - box2.MinX);
            margin += (box1.MaxY - box1.MinY);
            margin += (box2.MaxY - box2.MinY);
            return margin;
        }
        protected virtual Double GetOverlap(Node node1, Node node2)
        {
            MinimumBoundingBox overlapArea =
                IntersectMinimumBoundingBoxes(node1.CalculateMinimumBoundingBox(), node2.CalculateMinimumBoundingBox());
            return (overlapArea.MaxX - overlapArea.MinX) *
                (overlapArea.MaxY - overlapArea.MinY);
        }
        protected virtual void ReInsert(Node node, Int32 level)
        {
            MinimumBoundingBox nodeBox = node.CalculateMinimumBoundingBox();
            SortedList<Double, NodeEntry> distances = new SortedList<Double, NodeEntry>(),
                reInsertions = new SortedList<Double, NodeEntry>();;
            foreach (NodeEntry entry in node.NodeEntries)
                distances.Add(GetCenterDistance(nodeBox, entry.MinimumBoundingBox)*-1, entry);
            for (int i = 0; i < NumberOfEntriesForReInsert; i++)
                reInsertions.Add(distances.Keys[i] * -1, distances[distances.Keys[i]]);
            foreach (NodeEntry entry in reInsertions.Values)
                node.RemoveNodeEntry(entry);
            AdjustTree(node, level);
            foreach (NodeEntry entry in reInsertions.Values)
                Insert(entry, level);
        }
        protected override void AdjustTree(Node node1, Node node2)
        {
            AdjustTree(node1, node2, CalculateHeight(node1));
        }
        protected override void AdjustTree(Node node)
        {
            AdjustTree(node, null, CalculateHeight(node));
        }
        protected virtual void AdjustTree(Node node1, Node node2, Int32 level)
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
                if (parent.NodeEntries.Count > MaximumNodeOccupancy)
                {
                    if (OverflowMarkers.Contains(level))
                    {
                        List<Node> splitNodes = Split(parent);
                        if (parent == Root)
                            Root = null;
                        RemoveFromParent(parent);
                        AdjustTree(splitNodes[0], splitNodes[1], level - 1);
                        return;
                    }
                    else
                    {
                        OverflowMarkers.Add(level);
                        ReInsert(parent);
                        AdjustTree(parent, level);
                    }
                }
            }
            AdjustTree(parent, level - 1);
        }
        protected virtual void AdjustTree(Node node, Int32 level)
        {
            AdjustTree(node, null, level);
        }
        protected virtual Double GetCenterDistance(MinimumBoundingBox box1, MinimumBoundingBox box2)
        {
            return GetDistance(box1.MaxX - box1.MinX, box1.MaxY - box1.MinY, box2.MaxX - box2.MinX, box2.MaxY - box2.MinY);
        }
    }
}
