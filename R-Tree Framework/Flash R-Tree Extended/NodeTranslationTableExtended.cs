using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.Indexes
{
    public class NodeTranslationTableExtended : NodeTranslationTable
    {
        #region Constructors

        public NodeTranslationTableExtended(CacheManager underlyingCache)
            :base(underlyingCache)
        {
        }
        public NodeTranslationTableExtended(String tableSaveLocation, CacheManager underlyingCache)
            :base(tableSaveLocation, underlyingCache)
        {
        }

        #endregion
        #region Protected Methods

        protected override List<Sector> GenerateSectors(List<IndexUnit> indexUnits)
        {
            SortedList<Address, List<IndexUnit>> groupings = new SortedList<Address, List<IndexUnit>>();
            foreach (IndexUnit indexUnit in indexUnits)
            {
                if (!groupings.ContainsKey(indexUnit.Node))
                    groupings.Add(indexUnit.Node, new List<IndexUnit>());
                groupings[indexUnit.Node].Add(indexUnit);
            }
            List<Sector> sectors = new List<Sector>();
            List<List<IndexUnit>> newGroupings = new List<List<IndexUnit>>();
            while (groupings.Count > 0)
            {
                List<IndexUnit> grouping = groupings[groupings.Keys[0]];
                if (grouping.Count > Constants.INDEX_UNIT_ENTRIES_PER_SECTOR)
                {
                    Pair<List<IndexUnit>, List<IndexUnit>> subGroupings = SplitGrouping(grouping);
                    newGroupings.Add(subGroupings.Value1);
                    newGroupings.Add(subGroupings.Value2);
                }
                else
                    newGroupings.Add(grouping);
                groupings.RemoveAt(groupings.IndexOfValue(grouping));
            }
            while (newGroupings.Count > 0)
            {
                Sector newSector = new Sector(Constants.INDEX_UNIT_ENTRIES_PER_SECTOR);
                List<IndexUnit> initialGrouping = newGroupings[0];
                newGroupings.Remove(initialGrouping);
                foreach (IndexUnit i in initialGrouping)
                    newSector.AddIndexUnit(i);
                while (newSector.IndexUnits.Count <= Constants.INDEX_UNIT_ENTRIES_PER_SECTOR &&
                    newGroupings.Count > 0)
                {
                    List<List<IndexUnit>> eligibleGroupings = new List<List<IndexUnit>>();
                    foreach (List<IndexUnit> grouping in newGroupings)
                        if (grouping.Count + newSector.IndexUnits.Count <= Constants.INDEX_UNIT_ENTRIES_PER_SECTOR)
                            eligibleGroupings.Add(grouping);
                    if (eligibleGroupings.Count == 0)
                        break;
                    List<IndexUnit> closestGrouping = PickClosestGrouping(initialGrouping, eligibleGroupings);
                    foreach (IndexUnit i in closestGrouping)
                        newSector.AddIndexUnit(i);
                    initialGrouping.AddRange(closestGrouping);
                    newGroupings.Remove(closestGrouping);
                }
            }
            return sectors;
        }
        protected virtual List<IndexUnit> PickClosestGrouping(List<IndexUnit> grouping, List<List<IndexUnit>> possibleGroupings)
        {
            List<IndexUnit> closestGrouping = possibleGroupings[0];
            MinimumBoundingBox mbb = CalculateMinimumBoundingBox(grouping);
            Single minDistance = GetCenterDistance(mbb, CalculateMinimumBoundingBox(closestGrouping));
            foreach (List<IndexUnit> possibleGrouping in possibleGroupings)
            {
                Single distance = GetCenterDistance(mbb, CalculateMinimumBoundingBox(possibleGrouping));
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestGrouping = possibleGrouping;
                }
            }
            return closestGrouping;
        }
        protected virtual Single GetCenterDistance(MinimumBoundingBox box1, MinimumBoundingBox box2)
        {
            return GetDistance((box1.MaxX + box1.MinX) / 2, (box1.MaxY + box1.MinY) / 2, (box2.MaxX + box2.MinX) / 2, (box2.MaxY + box2.MinY) / 2);
        }
        protected virtual Pair<IndexUnit, IndexUnit> PickSeeds(List<IndexUnit> seedPool)
        {
            IndexUnit worstPairEntry1, worstPairEntry2;
            worstPairEntry1 = seedPool[0];
            worstPairEntry2 = seedPool[1];
            Single worstEnlargement = GetFutureSize(worstPairEntry1.ChildBoundingBox, worstPairEntry2.ChildBoundingBox) -
                        worstPairEntry1.ChildBoundingBox.GetArea() - worstPairEntry2.ChildBoundingBox.GetArea();
            for (int i = 0; i < seedPool.Count; i++)
                for (int j = i + 1; j < seedPool.Count; j++)
                {
                    Single d = GetFutureSize(seedPool[i].ChildBoundingBox, seedPool[j].ChildBoundingBox) -
                        seedPool[i].ChildBoundingBox.GetArea() - seedPool[j].ChildBoundingBox.GetArea();
                    if (d > worstEnlargement)
                    {
                        worstPairEntry1 = seedPool[i];
                        worstPairEntry2 = seedPool[j];
                        worstEnlargement = d;
                    }
                }
            return new Pair<IndexUnit, IndexUnit>(worstPairEntry1, worstPairEntry2);
        }
        protected virtual Single GetFutureSize(MinimumBoundingBox area1, MinimumBoundingBox area2)
        {
            MinimumBoundingBox futureMinimumBoundingBox = CombineMinimumBoundingBoxes(area1, area2);
            return (futureMinimumBoundingBox.MaxX - futureMinimumBoundingBox.MinX) *
                (futureMinimumBoundingBox.MaxX - futureMinimumBoundingBox.MinX) +
                (futureMinimumBoundingBox.MaxY - futureMinimumBoundingBox.MinY) *
                (futureMinimumBoundingBox.MaxY - futureMinimumBoundingBox.MinY);
        }
        protected virtual MinimumBoundingBox CalculateMinimumBoundingBox(List<IndexUnit> indexUnits)
        {
            MinimumBoundingBox mbb = indexUnits[0].ChildBoundingBox;
            foreach (IndexUnit i in indexUnits)
            {
                if (i.ChildBoundingBox.MaxX > mbb.MaxX)
                    mbb.MaxX = i.ChildBoundingBox.MaxX;
                if (i.ChildBoundingBox.MaxY > mbb.MaxY)
                    mbb.MaxX = i.ChildBoundingBox.MaxY;
                if (i.ChildBoundingBox.MinX < mbb.MinX)
                    mbb.MaxX = i.ChildBoundingBox.MinX;
                if (i.ChildBoundingBox.MinY < mbb.MinY)
                    mbb.MaxX = i.ChildBoundingBox.MinY;
            }
            return mbb;
        }
        protected virtual Pair<List<IndexUnit>, List<IndexUnit>> SplitGrouping(List<IndexUnit> grouping)
        {
            Pair<IndexUnit, IndexUnit> seeds = PickSeeds(grouping);
            grouping.Remove(seeds.Value1);
            grouping.Remove(seeds.Value2);
            List<IndexUnit> grouping1 = new List<IndexUnit>(), grouping2 = new List<IndexUnit>();
            grouping1.Add(seeds.Value1);
            grouping2.Add(seeds.Value2);
            while (grouping.Count > 0)
            {
                MinimumBoundingBox minimumBoundingBox1 = CalculateMinimumBoundingBox(grouping1),
                minimumBoundingBox2 = CalculateMinimumBoundingBox(grouping2);
                IndexUnit nextEntry = PickNext(grouping, minimumBoundingBox1, minimumBoundingBox2);
                grouping.Remove(nextEntry);
                List<IndexUnit> groupToEnter;
                if (GetFutureSize(nextEntry.ChildBoundingBox, minimumBoundingBox1) ==
                    GetFutureSize(nextEntry.ChildBoundingBox, minimumBoundingBox2))
                {
                    if (minimumBoundingBox1.GetArea() == minimumBoundingBox2.GetArea())
                        if (grouping1.Count <= grouping2.Count)
                            groupToEnter = grouping1;
                        else
                            groupToEnter = grouping2;
                    else if (minimumBoundingBox1.GetArea() < minimumBoundingBox2.GetArea())
                        groupToEnter = grouping1;
                    else
                        groupToEnter = grouping2;
                }
                else if (GetFutureSize(nextEntry.ChildBoundingBox, minimumBoundingBox1) <
                    GetFutureSize(nextEntry.ChildBoundingBox, minimumBoundingBox2))
                    groupToEnter = grouping1;
                else
                    groupToEnter = grouping2;
                groupToEnter.Add(nextEntry);
            }
            return new Pair<List<IndexUnit>, List<IndexUnit>>(grouping1, grouping2);
        }
        protected virtual IndexUnit PickNext(List<IndexUnit> entryPool, MinimumBoundingBox minimumBoundingBox1, MinimumBoundingBox minimumBoundingBox2)
        {
            IndexUnit nextEntry = entryPool[0];

            Single maxEnlargementDifference = Math.Abs(
                GetFutureSize(nextEntry.ChildBoundingBox, minimumBoundingBox1) -
                GetFutureSize(nextEntry.ChildBoundingBox, minimumBoundingBox2));
            foreach (IndexUnit entry in entryPool)
            {
                Single enlargmentDifference = Math.Abs(
                GetFutureSize(entry.ChildBoundingBox, minimumBoundingBox1) -
                GetFutureSize(entry.ChildBoundingBox, minimumBoundingBox2));
                if (enlargmentDifference > maxEnlargementDifference)
                {
                    maxEnlargementDifference = enlargmentDifference;
                    nextEntry = entry;
                }
            }
            return nextEntry;
        }
        protected virtual MinimumBoundingBox CombineMinimumBoundingBoxes(MinimumBoundingBox area1, MinimumBoundingBox area2)
        {
            Single newMinX, newMaxX, newMinY, newMaxY;
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
        protected virtual Single GetDistance(Single x1, Single y1, Single x2, Single y2)
        {
            return (((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2)));
        }

        #endregion
    }
}
