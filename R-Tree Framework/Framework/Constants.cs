using System;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public static class Constants
    {
        public const Int32
            ADDRESS_SIZE = 4,
            PAGE_SIZE = 4096, 
            NODE_ENTRY_SIZE = 16 + ADDRESS_SIZE,
            NODE_SIZE = PAGE_SIZE,
            RECORD_SIZE = PAGE_SIZE,
            NODE_OVERHEAD = ADDRESS_SIZE + 4,
            NODE_ENTRIES_PER_NODE = (PAGE_SIZE - NODE_OVERHEAD) / NODE_ENTRY_SIZE;

    }
}