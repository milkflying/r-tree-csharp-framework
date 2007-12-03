using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.Indexes
{
    public class R_Star_Tree : R_Tree
    {
        public R_Star_Tree(Int32 minimumNodeOccupancy, Int32 maximumNodeOccupancy, CacheManager cache)
            : base(minimumNodeOccupancy, maximumNodeOccupancy, cache)
        {
        }

        protected override Leaf ChooseLeaf(Record record)
        {
            Node node = Root;
            while (!(node is Leaf))
            {
                if (node.ChildType.Equals(typeof(Leaf)))
                {
                    NodeEntry minOverlap = node.NodeEntries[0];
                    Double minOverlapArea = GetFutureOverlap(record, minOverlap, node);
                    foreach (NodeEntry nodeEntry in node.NodeEntries)
                    {
                        Double overlap = GetFutureOverlap(record, nodeEntry, node);
                        if (overlap == minOverlapArea)
                        {
                            Double nodeEntryFutureSize = GetFutureSize(record, nodeEntry.MinimumBoundingBox),
                                nodeEntrySize = nodeEntry.MinimumBoundingBox.GetArea(),
                                minOverlapFutureSize = GetFutureSize(record, minOverlap.MinimumBoundingBox),
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
                    node = Cache.LookupNode(minOverlap.Child);
                }
                else
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
                    node = Cache.LookupNode(minEnlargment.Child);
                }
            }
            return node as Leaf;
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
        protected virtual Double GetFutureOverlap(Record record, NodeEntry insertionEntry, Node node)
        {
            MinimumBoundingBox overlapMinimumBoundingBox =
                CombineMinimumBoundingBoxes(record.MinimumBoundingBox, insertionEntry.MinimumBoundingBox);
            foreach (NodeEntry nodeEntry in node.NodeEntries)
                if (nodeEntry != insertionEntry)
                    overlapMinimumBoundingBox = IntersectMinimumBoundingBoxes(overlapMinimumBoundingBox, nodeEntry.MinimumBoundingBox);
            return (overlapMinimumBoundingBox.MaxX - overlapMinimumBoundingBox.MinX) *
                (overlapMinimumBoundingBox.MaxY - overlapMinimumBoundingBox.MinY);
        }
        protected virtual MinimumBoundingBox IntersectMinimumBoundingBoxes(MinimumBoundingBox area1, MinimumBoundingBox area2)
        {
            if(!Overlaps(area1, area2))
                return new MinimumBoundingBox(0,0,0,0);
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
    }
}
