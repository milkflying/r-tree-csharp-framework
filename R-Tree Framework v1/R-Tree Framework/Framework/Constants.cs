using System;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public static class Constants
    {
        public const Int32
            ADDRESS_SIZE = 4,
            PAGE_SIZE = 4096,

            NODE_ENTRY_SIZE = 16 + ADDRESS_SIZE,
            INDEX_UNIT_SIZE = 17 + 2 * ADDRESS_SIZE,

            NODE_SIZE = PAGE_SIZE,
            RECORD_SIZE = PAGE_SIZE,
            SECTOR_SIZE = PAGE_SIZE,

            NODE_OVERHEAD = ADDRESS_SIZE + 4,
            SECTOR_OVERHEAD = 3,

            MAXIMUM_ENTRIES_PER_NODE = (PAGE_SIZE - NODE_OVERHEAD) / NODE_ENTRY_SIZE,
            MINIMUM_ENTRIES_PER_NODE = MAXIMUM_ENTRIES_PER_NODE * 3 / 10,
            INDEX_UNIT_ENTRIES_PER_SECTOR = (PAGE_SIZE - SECTOR_OVERHEAD) / INDEX_UNIT_SIZE,
            
            SECTOR_LIST_LENGTH = 2,
            NODES_FOR_REINSERT = MAXIMUM_ENTRIES_PER_NODE * 3 / 10 + ((MAXIMUM_ENTRIES_PER_NODE * 3) % 10 > 4 ? 1 : 0);

    }

    public enum PageDataType : byte { Node, Leaf, Record, IndexUnitSector }
    public enum NodeChildType : byte { Node, Leaf, Record }
    public enum Operation : byte { Insert, Update, Delete };
}