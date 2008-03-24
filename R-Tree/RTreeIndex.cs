using System;
using System.Collections.Generic;
using System.Text;
using R_Tree_Framework.Index;
using R_Tree_Framework.Index.Exceptions;
using R_Tree_Framework.Query;
using R_Tree_Framework.Utility;

namespace R_Tree
{
    public class RTreeIndex<CoordinateType> : SpatialIndex<CoordinateType> where CoordinateType : struct, IComparable
    {
        protected Int32 maxEntriesPerInteriorNode, maxEntriesPerLeafNode, minEntriesPerInteriorNode, minEntriesPerLeafNode;

        public virtual Int32 MinEntriesPerInteriorNode
        {
            get { return minEntriesPerInteriorNode; }
            protected set { minEntriesPerInteriorNode = value; }
        }
        public virtual Int32 MaxEntriesPerInteriorNode
        {
            get { return maxEntriesPerInteriorNode; }
            protected set { maxEntriesPerInteriorNode = value; }
        }
        public virtual Int32 MaxEntriesPerLeafNode
        {
            get { return maxEntriesPerLeafNode; }
            protected set { maxEntriesPerLeafNode = value; }
        }
        public virtual Int32 MinEntriesPerLeafNode
        {
            get { return minEntriesPerLeafNode; }
            protected set { minEntriesPerLeafNode = value; }
        }

        public RTreeIndex(Int32 dimension, Int32 pageSize, Double minPercentOfMaxInteriorNodeEntries, Double minPercentOfMaxLeafNodeEntries)
            : base(dimension, pageSize)
        {
            MaxEntriesPerInteriorNode = PageSize / InteriorNodeEntry<CoordinateType>.GetSize(Dimension);
            MaxEntriesPerLeafNode = PageSize / LeafNodeEntry<CoordinateType>.GetSize(Dimension);
            MinEntriesPerInteriorNode = Convert.ToInt32(MaxEntriesPerInteriorNode * minPercentOfMaxInteriorNodeEntries + .5);
            MinEntriesPerLeafNode = Convert.ToInt32(MaxEntriesPerLeafNode * minPercentOfMaxLeafNodeEntries + .5);
        }

        public override List<Int32> Search(Query query)
        {
            if (query is SpatialQuery)
                return Search(query as SpatialQuery);
            else
                throw new QueryTypeNotSupportedException();
        }

        public override List<Int32> Search(SpatialQuery query)
        {
            if (query is RegionQuery)
                return Search(query as RegionQuery, RootNode);
            else if (query is RelativeQuery)
                return Search(query as RelativeQuery, RootNode);
            else
                throw new QueryTypeNotSupportedException();
        }
        protected virtual List<Int32> Search(RegionQuery query, Node<CoordinateType> rootOfTree)
        {
            List<Int32> results = new List<Int32>();
            foreach(NodeEntry<CoordinateType> nodeEntry in rootOfTree.NodeEntries)
                if(Overlaps(query, nodeEntry.MinimumBoundingBox))
                    if(rootOfTree is LeafNode<CoordinateType>)
                        results.Add((nodeEntry as LeafNodeEntry<CoordinateType>).RecordID);
                    else
                        results.AddRange(Search(query, CacheManager.LookUpNode((nodeEntry as InteriorNodeEntry<CoordinateType>).ChildNode)));
            return results;
        }

        protected virtual Boolean Overlaps(RegionQuery query, MinimumBoundingBox<CoordinateType> boundingBox)
        {
            if (query is WindowQuery)
                return Overlaps(query as WindowQuery, boundingBox);
            else if (query is RangeQuery)
                return Overlaps(query as RangeQuery, boundingBox);
            else
                throw new QueryTypeNotSupportedException();
        }

        protected virtual Boolean Overlaps(WindowQuery query, MinimumBoundingBox<CoordinateType> boundingBox)
        {
            
        }

        protected virtual Boolean Overlaps(RangeQuery query, MinimumBoundingBox<CoordinateType> boundingBox)
        {
        }

    }
}
