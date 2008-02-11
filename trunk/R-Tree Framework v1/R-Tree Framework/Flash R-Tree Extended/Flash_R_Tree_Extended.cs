using System;
using System.Collections.Generic;
using System.Text;
using Edu.Psu.Cse.R_Tree_Framework.Framework;

namespace Edu.Psu.Cse.R_Tree_Framework.Indexes
{
    public class Flash_R_Tree_Extended : Flash_R_Tree
    {
        public Flash_R_Tree_Extended(CacheManager cache, Int32 reservationBufferSize)
            : base(cache, reservationBufferSize)
        {
            NodeTranslationTable = new NodeTranslationTableExtended(cache);
            Cache = NodeTranslationTable;
        }
        public Flash_R_Tree_Extended(String savedFileLocation, CacheManager cache)
            : base(savedFileLocation, cache)
        {
            NodeTranslationTable = new NodeTranslationTableExtended(savedFileLocation + ".ntt", cache);
            Cache = NodeTranslationTable;
        }
    }
}
