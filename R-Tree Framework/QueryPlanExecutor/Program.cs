using System;
using System.Collections.Generic;
using System.Text;

namespace QueryPlanExecutor
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            String 
                queryPlanFileLocation = args[0], 
                resultSaveFileLocation= args[1], 
                savedCacheFileLocation= args[2],
                savedIndexFileLocation = args[3];
            Type
                cacheType = Type.GetType(args[4]),
                treeType = Type.GetType(args[5]);
            Int32
                cacheSize = Int32.Parse(args[6]);
            QueryPlanExecutor
                queryPlanExecutor = new QueryPlanExecutor(
                    queryPlanFileLocation, 
                    resultSaveFileLocation, 
                    savedCacheFileLocation, 
                    savedIndexFileLocation, 
                    cacheType, 
                    treeType, 
                    cacheSize);
            queryPlanExecutor.ExecuteQueryPlan();
        }
    }
}
