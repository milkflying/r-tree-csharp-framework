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
                resultSaveFileLocation = args[1],
                savedCacheFileLocation = args[2],
                savedIndexFileLocation = args[3],
                databaseRunFileLocation = args[4];
            Type
                cacheType = Type.GetType(args[5]),
                treeType = Type.GetType(args[6]);
            QueryPlanExecutor
                queryPlanExecutor = new QueryPlanExecutor(
                    queryPlanFileLocation, 
                    resultSaveFileLocation, 
                    savedCacheFileLocation, 
                    savedIndexFileLocation,
                    databaseRunFileLocation,
                    cacheType, 
                    treeType);
            queryPlanExecutor.ExecuteQueryPlan();
        }
    }
}
