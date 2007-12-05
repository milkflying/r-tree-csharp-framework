using System;

namespace Edu.Psu.Cse.R_Tree_Framework.Framework
{
    public static class Constants
    {
        public const Int32
            PAGE_SIZE = 4096, 
            NODE_ENTRY_SIZE = 48,
            NODE_SIZE = PAGE_SIZE,
            RECORD_SIZE = PAGE_SIZE,
            NODE_OVERHEAD = 52,
            NODE_ENTRIES_PER_NODE = (PAGE_SIZE - NODE_OVERHEAD) / NODE_ENTRY_SIZE;

    }
}